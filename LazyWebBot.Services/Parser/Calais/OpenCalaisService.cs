namespace LazyWebBot.Services.Parser.Calais
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Xml.Linq;

    using LazyWebBot.Services.Exceptions;

    using OpenCalais.API;
    using OpenCalais.Domain;

    public class OpenCalaisService : IOpenCalaisService
    {
        public string LicenseKey { get; set; }

        public string GetUsefulWords(string content)
        {
            try
            {
                var xml = GetXml(LicenseKey, content);
                var instances = ParseToInstances(xml);

                var instanceInfo = instances.Where(o => o as InstanceInfo != null).OrderByDescending(i => ((InstanceInfo)i).Exact.Length).FirstOrDefault() as InstanceInfo;
                return instanceInfo != null ? instanceInfo.Exact : null;
            }
            catch (Exception ex)
            {
                throw new OpenCalaisException(ex);
            }
        }

        private static IEnumerable<object> ParseToInstances(XContainer xml)
        {
            // warning: hold onto your hats.. this is some gnarly code..
            var rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

            var descriptions = xml.Descendants("rdf" + "Description");
            var instances = new List<object>();

            foreach (var d in descriptions)
            {
                var resource = d.Element(string.Format("{0}type", rdf)).Attribute(string.Format("{0}resource", rdf)).Value;
                var domain = Assembly.Load("OpenCalais");

                var type = (from t in domain.GetTypes()
                            where
                                t.Namespace.Contains("Domain") && t.IsClass
                                && t.GetCustomAttributes(typeof(RDFAbout), true).Count() == 1
                                && (t.GetCustomAttributes(typeof(RDFAbout), true).First() as RDFAbout).Value.Trim().ToLower()
                                == resource.Trim().ToLower()
                            select t).FirstOrDefault();

                if (type == null)
                {
                    continue;
                }

                var instance = Activator.CreateInstance(type);
                if (d.Attributes(rdf + "about").Count() == 1)
                {
                    ((Resource)instance).AboutUri = d.Attribute(rdf + "about").Value;
                }

                var props = type.GetProperties().Where(p => p.Name != "AboutUri");

                foreach (var prop in props)
                {
                    var propUrl = (prop.GetCustomAttributes(typeof(RDFAbout), true).FirstOrDefault() as RDFAbout).Value;

                    XNamespace c = "http://s.opencalais.com/1/pred/";
                    var name = propUrl.Split('/').Last();
                    var propElement = d.Element(c + name.ToLower());

                    if (prop.PropertyType == typeof(string) && propElement != null && !string.IsNullOrEmpty(propElement.Value))
                    {
                        prop.SetValue(instance, propElement.Value, null);
                    }

                    if (prop.PropertyType != typeof(DateTime) || propElement == null || string.IsNullOrEmpty(propElement.Value))
                    {
                        continue;
                    }

                    try
                    {
                        prop.SetValue(instance, DateTime.Parse(propElement.Value), null);
                    }
                    catch (Exception)
                    {
                    }
                }

                instances.Add(instance);
            }
            return instances;
        }

        private static XElement GetXml(string licenseId, string content)
        {
            var calais = new calais();
            const string ParamsXml = "<c:params xmlns:c=\"http://s.opencalais.com/1/pred/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">" +
                                     "<c:processingDirectives c:contentType=\"Text/HTML\" c:outputFormat=\"XML/RDF\" c:calculateRelevanceScore=\"True\" c:enableMetadataType=\"GenericRelations\" />" +
                                     "<c:userDirectives c:allowDistribution=\"False\" c:allowSearch=\"False\" c:submitter=\"MWS\" /></c:params>";
            var resp = calais.Enlighten(licenseId, HttpUtility.HtmlEncode(content.Replace(".", string.Empty)), ParamsXml);
            return XElement.Parse(resp);
        }
    }
}
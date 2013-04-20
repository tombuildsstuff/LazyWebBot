namespace LazyWebBot.Services.Parser.Wolfram
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using LazyWebBot.Services.Exceptions;

    using WolframAPI;

    public class WolframAlphaService : IWolframAlphaService
    {
        private readonly string _licenseKey;

        public WolframAlphaService(string licenseKey)
        {
            _licenseKey = licenseKey;
        }

        public string GetUsefulWords(string question)
        {
            try
            {
                var client = new WAClient(_licenseKey);
                var solution = client.Submit(question);
                if (question != null)
                {
                    var document = XElement.Parse(solution);
                    var result = document.Descendants("pod").FirstOrDefault(p => p.Attribute("title").Value == "Result");
                    var value = result != null ? result.Descendants("plaintext").FirstOrDefault() : null;
                    return value != null ? value.Value : null;
                }

                return solution;
            }
            catch (Exception ex)
            {
                throw new WolframAlphaException(ex);
            }
        }
    }
}
namespace LazyWebBot.Parser
{
    using System.Configuration;

    using LazyWebBot.Services.Parser;
    using LazyWebBot.Services.Parser.Calais;
    using LazyWebBot.Services.Parser.Wolfram;
    using LazyWebBot.Services.Search;

    using NServiceBus;
    using NServiceBus.Config;

    public class ParserInstaller : INeedInitialization
    {
        public void Init()
        {
            var openCalaisKey = ConfigurationManager.AppSettings["OpenCalaisLicense"];
            var wolframAlphaKey = ConfigurationManager.AppSettings["WolframAlphaLicense"];

            Configure.Instance.Configurer.ConfigureComponent<OpenCalaisService>(DependencyLifecycle.InstancePerCall).ConfigureProperty(c => c.LicenseKey, openCalaisKey);
            Configure.Instance.Configurer.ConfigureComponent<WolframAlphaService>(DependencyLifecycle.InstancePerCall).ConfigureProperty(c => c.LicenseKey, wolframAlphaKey);
            Configure.Instance.Configurer.ConfigureComponent<SearchService>(DependencyLifecycle.InstancePerCall);
            Configure.Instance.Configurer.ConfigureComponent<TextParser>(DependencyLifecycle.InstancePerCall);
        }
    }
}
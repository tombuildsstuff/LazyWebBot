namespace LazyWebBot.Parser
{
    using LazyWebBot.Services.Search;

    using NServiceBus;
    using NServiceBus.Config;

    public class ParserInstaller : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance.Configurer.ConfigureComponent<SearchService>(DependencyLifecycle.InstancePerCall);
            Configure.Instance.Configurer.ConfigureComponent<FakeTextParser>(DependencyLifecycle.InstancePerCall);
        }
    }
}
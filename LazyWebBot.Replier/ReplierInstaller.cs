namespace LazyWebBot.Replier
{
    using System.Configuration;

    using NServiceBus;
    using NServiceBus.Config;

    using TweetSharp;

    public class ReplierInstaller : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance.Configurer.ConfigureComponent<ITwitterService>(() =>
            {
                // return new FakeTwitterService();

                var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
                var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                var userAccessToken = ConfigurationManager.AppSettings["userAccessToken"];
                var userTokenSecret = ConfigurationManager.AppSettings["userTokenSecret"];

                var service = new TwitterService(consumerKey, consumerSecret);
                service.AuthenticateWith(userAccessToken, userTokenSecret);
                return service;
            },
            DependencyLifecycle.InstancePerUnitOfWork);
        }
    }
}
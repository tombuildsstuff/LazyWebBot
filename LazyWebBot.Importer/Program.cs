namespace LazyWebBot.Importer
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using LazyWebBot.Contracts;

    using NServiceBus;

    using Raven.Client;
    using Raven.Client.Document;

    using TweetSharp;

    public class Program
    {
        private static IBus Bus
        {
            get
            {
                return Configure.With().DefaultBuilder().XmlSerializer().MsmqTransport().UnicastBus().SendOnly();
            }
        }

        private static IDocumentStore DocumentStore
        {
            get
            {
                return new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "LazyWebBot.Tweets" };
            }
        }

        public static void Main(string[] args)
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            var userAccessToken = ConfigurationManager.AppSettings["userAccessToken"];
            var userTokenSecret = ConfigurationManager.AppSettings["userTokenSecret"];
            var searchTerm = ConfigurationManager.AppSettings["searchTerm"];

            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(userAccessToken, userTokenSecret);
            VerifyUser(service);

            var documentStore = DocumentStore.Initialize();

            while (true)
            {
                RunSearch(service, searchTerm, Bus, documentStore.OpenSession());
                Thread.Sleep(15000);
            }
        }

        private static void RunSearch(ITwitterService service, string searchTerm, IBus bus, IDocumentSession documentSession)
        {
            var options = new TweetSharp.SearchOptions { Q = searchTerm, Lang = "en", Resulttype = TwitterSearchResultType.Mixed };
            var results = service.Search(options);
            foreach (var result in results.Statuses)
            {
                var tweet = new Status
                {
                    Id = Guid.NewGuid(),
                    TwitterId = result.Id,
                    User = result.User.ScreenName,
                    UserId = result.User.Id,
                    Content = result.Text,
                    Created = result.CreatedDate
                };

                if (result.InReplyToStatusId.HasValue || documentSession.Query<Status>().Any(s => s.TwitterId == tweet.TwitterId))
                {
                    return;
                }

                documentSession.Store(tweet);
                documentSession.SaveChanges();
                bus.Send(tweet);
            }
        }

        private static void VerifyUser(ITwitterService service)
        {
            var user = service.VerifyCredentials(new VerifyCredentialsOptions());
            Console.WriteLine("{0} ({1})", user.Name, user.ScreenName);
        }
    }
}
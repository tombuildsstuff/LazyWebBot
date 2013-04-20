namespace LazyWebBot.Playground
{
    using System;
    using System.Configuration;

    using AutoMapper;

    using LazyWebBot.Contracts;

    using NServiceBus;

    using TweetSharp;

    public class Program
    {
        public static void Main(string[] args)
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            var userAccessToken = ConfigurationManager.AppSettings["userAccessToken"];
            var userTokenSecret = ConfigurationManager.AppSettings["userTokenSecret"];
            var searchTerm = ConfigurationManager.AppSettings["searchTerm"];

            var bus = GetBus();

            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(userAccessToken, userTokenSecret);
            VerifyUser(service);
            
            RunSearch(service, searchTerm, bus);
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.CreateMap<TwitterStatus, Status>()
                .ForMember(x => x.Content, y => y.ResolveUsing(f => f.Text))
                .ForMember(x => x.Created, y => y.ResolveUsing(f => f.CreatedDate))
                .ForMember(x => x.TwitterId, y => y.ResolveUsing(f => f.Id));
        }

        private static IBus GetBus()
        {
            return Configure.With().DefaultBuilder().XmlSerializer().MsmqTransport().UnicastBus().SendOnly();
        }

        private static void RunSearch(ITwitterService service, string searchTerm, IBus bus)
        {
            var results = service.Search(new SearchOptions { Q = searchTerm, Lang = "en", Resulttype = TwitterSearchResultType.Mixed });
            foreach (var result in results.Statuses)
            {
                if (result.InReplyToStatusId.HasValue)
                    return;

                var tweet = new Status
                {
                    Id = Guid.NewGuid(),
                    TwitterId = result.Id,
                    User = result.User.ScreenName,
                    UserId = result.User.Id,
                    Content = result.Text,
                    Created = result.CreatedDate
                };
                bus.Send(tweet);
            }

            Console.ReadLine();
        }

        private static void VerifyUser(ITwitterService service)
        {
            var user = service.VerifyCredentials(new VerifyCredentialsOptions());
            Console.WriteLine("{0} ({1})", user.Name, user.ScreenName);
        }
    }
}
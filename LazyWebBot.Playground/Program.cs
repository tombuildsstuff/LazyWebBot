namespace LazyWebBot.Playground
{
    using System;
    using System.Configuration;

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

            var service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(userAccessToken, userTokenSecret);
            VerifyUser(service);
            
            RunSearch(service, searchTerm);
        }

        private static void RunSearch(ITwitterService service, string searchTerm)
        {
            var results = service.Search(new SearchOptions { Q = searchTerm, Lang = "en", Resulttype = TwitterSearchResultType.Mixed });
            foreach (var result in results.Statuses)
            {
                Console.WriteLine(result.Text);
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
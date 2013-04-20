namespace LazyWebBot.Replier
{
    using System;

    using TweetSharp;

    public class FakeTwitterService : TwitterService
    {
        public override TwitterStatus SendTweet(SendTweetOptions options)
        {
            Console.WriteLine(options.Status);
            return new TwitterStatus();
        }
    }
}
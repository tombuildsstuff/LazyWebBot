namespace LazyWebBot.Replier
{
    using LazyWebBot.Contracts.Replier;

    using NServiceBus;

    using TweetSharp;

    public class RecommendationHandler : IHandleMessages<Recommendation>
    {
        public ITwitterService TwitterService { get; set; }

        public void Handle(Recommendation message)
        {
            TwitterService.SendTweet(new SendTweetOptions { InReplyToStatusId = message.TwitterId, Status = message.ToString() });
        }
    }
}
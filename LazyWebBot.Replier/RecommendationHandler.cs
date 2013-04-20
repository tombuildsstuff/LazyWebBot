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
            var status = string.Format("@{0} LazyWebBot does the searching for you! {1}", message.User, message.RecommendationUrl);
            TwitterService.SendTweet(new SendTweetOptions { InReplyToStatusId = message.TwitterId, Status = status });
        }
    }
}
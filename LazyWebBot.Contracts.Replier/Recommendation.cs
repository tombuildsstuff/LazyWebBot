namespace LazyWebBot.Contracts.Replier
{
    using System;

    using NServiceBus;

    public class Recommendation : IMessage
    {
        public Guid Id { get; set; }

        public long TwitterId { get; set; }

        public string User { get; set; }

        public string OriginalTweet { get; set; }

        public string Answer { get; set; }

        public string RecommendationUrl { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Answer) ? string.Format("@{0} {1}", User, RecommendationUrl) : string.Format("@{0} {1} {2}", User, Answer, RecommendationUrl);
        }
    }
}
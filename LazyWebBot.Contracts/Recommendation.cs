namespace LazyWebBot.Contracts
{
    using System;

    public class Recommendation : IEntity
    {
        public Guid Id { get; set; }

        public Status OriginalTweet { get; set; }

        public string RecommendationUrl { get; set; }

        public DateTime Created { get; set; }
    }
}
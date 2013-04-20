namespace LazyWebBot.Parser
{
    using System;

    using LazyWebBot.Contracts;
    using LazyWebBot.Contracts.Replier;

    using NServiceBus;

    public class StatusParserHandler : IHandleMessages<Status>
    {
        public IBus Bus { get; set; }

        public void Handle(Status message)
        {
            Console.WriteLine("{0} - {1}", message.User, message.Content);
            Bus.Send(new Recommendation
            {
                Id = Guid.NewGuid(),
                OriginalTweet = message.Content,
                Created = DateTime.Now,
                RecommendationUrl = "http://example.com/product",
                TwitterId = message.TwitterId,
                User = message.User
            });
        }
    }
}
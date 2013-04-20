namespace LazyWebBot.Parser
{
    using System;

    using LazyWebBot.Contracts;
    using LazyWebBot.Contracts.Replier;
    using LazyWebBot.Services.Parser;
    using LazyWebBot.Services.Search;

    using NServiceBus;

    public class StatusParserHandler : IHandleMessages<Status>
    {
        public ISearchService SearchService { get; set; }

        public ITextParser TextParser { get; set; }

        public IBus Bus { get; set; }

        public void Handle(Status message)
        {
            var valuableWords = TextParser.GetValuableWords(message.Content);
            var combinedTerms = string.Join(" ", valuableWords);
            var url = SearchService.GetUriForTerm(combinedTerms);
            if (url == null)
            {
                Console.WriteLine("Unable to find a url to match {0}", combinedTerms);
                return;
            }

            Bus.Send(new Recommendation
            {
                Id = Guid.NewGuid(),
                OriginalTweet = message.Content,
                Created = DateTime.Now,
                RecommendationUrl = url.ToString(),
                TwitterId = message.TwitterId,
                User = message.User
            });
        }
    }
}
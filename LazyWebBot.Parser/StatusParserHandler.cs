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
            var sanitisedMessage = SanitiseInput(message);
            var terms = TextParser.GetValuableWords(sanitisedMessage);
            var url = SearchService.GetUriForTerm(terms);

            // if we can't find a url and we can't find any relevent details - there's no point in spamming the user..
            if (url == null && sanitisedMessage == terms)
            {
                Console.WriteLine("Unable to find a url to match {0}", terms);
                return;
            }

            Bus.Send(new Recommendation
            {
                Id = Guid.NewGuid(),
                OriginalTweet = message.Content,
                Created = DateTime.Now,
                RecommendationUrl = url != null ? url.ToString() : null,
                Answer = terms.Length > 100 ? null : terms,
                TwitterId = message.TwitterId,
                User = message.User
            });
        }

        private string SanitiseInput(Status status)
        {
            return status.Content.Replace(status.SearchTerm, string.Empty).Replace("#", string.Empty).Trim();
        }
    }
}
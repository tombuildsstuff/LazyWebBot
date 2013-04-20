namespace LazyWebBot.Parser
{
    using System;
    using System.Collections.Generic;

    using LazyWebBot.Services.Parser;

    public class FakeTextParser : ITextParser
    {
        public IList<string> GetValuableWords(string input)
        {
            return input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
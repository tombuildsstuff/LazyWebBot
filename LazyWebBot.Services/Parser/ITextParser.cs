namespace LazyWebBot.Services.Parser
{
    using System.Collections.Generic;

    public interface ITextParser
    {
        IList<string> GetValuableWords(string input);
    }
}
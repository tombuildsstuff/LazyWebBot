namespace LazyWebBot.Services.Parser
{
    using LazyWebBot.Services.Exceptions;
    using LazyWebBot.Services.Parser.Calais;
    using LazyWebBot.Services.Parser.Wolfram;

    public class TextParser : ITextParser
    {
        private readonly IOpenCalaisService _openCalais;

        private readonly IWolframAlphaService _wolframAlpha;

        public TextParser(IOpenCalaisService openCalais, IWolframAlphaService wolframAlpha)
        {
            _openCalais = openCalais;
            _wolframAlpha = wolframAlpha;
        }

        public string GetValuableWords(string input)
        {
            try
            {
                var words = _openCalais.GetUsefulWords(input);
                if (!string.IsNullOrWhiteSpace(words))
                    return words;
            }
            catch (OpenCalaisException)
            {
            }

            try
            {
                var words = _wolframAlpha.GetUsefulWords(input);
                if (!string.IsNullOrWhiteSpace(words))
                    return words;
            }
            catch (WolframAlphaException)
            {
            }

            return input;
        }
    }
}
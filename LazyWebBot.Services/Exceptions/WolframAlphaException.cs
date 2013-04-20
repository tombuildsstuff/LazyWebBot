namespace LazyWebBot.Services.Exceptions
{
    using System;

    public class WolframAlphaException : Exception
    {
        public WolframAlphaException(Exception ex) : base(string.Format("Wolfram Alpha Exception: {0}", ex.Message), ex)
        {
        }
    }
}
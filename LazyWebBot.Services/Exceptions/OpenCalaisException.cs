namespace LazyWebBot.Services.Exceptions
{
    using System;

    public class OpenCalaisException : Exception
    {
        public OpenCalaisException(Exception exception) : base (string.Format("OpenCalais Exception: {0}", exception.Message), exception)
        {
        }
    }
}
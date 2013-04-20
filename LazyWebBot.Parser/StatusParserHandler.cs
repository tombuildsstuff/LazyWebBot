namespace LazyWebBot.Parser
{
    using System;

    using LazyWebBot.Contracts;

    using NServiceBus;

    public class StatusParserHandler : IHandleMessages<Status>
    {
        public void Handle(Status message)
        {
            Console.WriteLine("{0} - {1}", message.User, message.Content);
        }
    }
}
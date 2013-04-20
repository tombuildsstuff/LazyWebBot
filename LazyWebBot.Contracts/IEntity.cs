namespace LazyWebBot.Contracts
{
    using System;

    using NServiceBus;

    public interface IEntity : IMessage
    {
        Guid Id { get; set; }

        DateTime Created { get; set; }
    }
}
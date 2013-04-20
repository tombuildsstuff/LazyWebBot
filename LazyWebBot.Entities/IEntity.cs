namespace LazyWebBot.Entities
{
    using System;

    public interface IEntity
    {
        Guid Id { get; set; }

        DateTime Created { get; set; }
    }
}
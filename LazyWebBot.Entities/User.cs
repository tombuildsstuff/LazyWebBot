namespace LazyWebBot.Entities
{
    using System;

    public class User : IEntity
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
    }
}
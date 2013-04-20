namespace LazyWebBot.Entities
{
    using System;

    public class Tweet : IEntity
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
    }
}
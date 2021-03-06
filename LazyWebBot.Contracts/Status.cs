﻿namespace LazyWebBot.Contracts
{
    using System;

    using NServiceBus;

    public class Status : IMessage
    {
        public Guid Id { get; set; }

        public long TwitterId { get; set; }

        public string User { get; set; }

        public string SearchTerm { get; set; }

        public long UserId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
    }
}
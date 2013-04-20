namespace LazyWebBot.Replier
{
    using LazyWebBot.Contracts;

    using NServiceBus;

    public class RecommendationHandler : IHandleMessages<Recommendation>
    {
        public void Handle(Recommendation message)
        {
            throw new System.NotImplementedException();
        }
    }
}
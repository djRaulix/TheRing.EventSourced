namespace TheRing.EventSourced.GetEventStore
{
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository;

    public class StreamNameFromAggregateRootGetter : IGetStreamNameFromAggregateRoot
    {
        public string Get(AggregateRoot aggregateRoot)
        {
            return string.Concat(aggregateRoot.GetType().Name, "_", aggregateRoot.Id);
        }
    }
}
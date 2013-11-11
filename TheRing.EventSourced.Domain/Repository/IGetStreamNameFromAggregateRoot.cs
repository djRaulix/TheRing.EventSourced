namespace TheRing.EventSourced.Domain.Repository
{
    using TheRing.EventSourced.Domain.Aggregate;

    public interface IGetStreamNameFromAggregateRoot
    {
        string Get(AggregateRoot aggregateRoot);
    }
}
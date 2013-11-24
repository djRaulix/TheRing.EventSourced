namespace TheRing.EventSourced.Domain.Repository.Snapshot
{
    using TheRing.EventSourced.Domain.Aggregate;

    public interface IGetSnapshotKeyFromAggregateRoot
    {
        string GetKey(AggregateRoot agg);
    }
}
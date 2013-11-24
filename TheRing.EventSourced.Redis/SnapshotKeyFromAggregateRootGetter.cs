namespace TheRing.EventSourced.Redis
{
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository.Snapshot;

    public class SnapshotKeyFromAggregateRootGetter : IGetSnapshotKeyFromAggregateRoot
    {
        public string GetKey(AggregateRoot agg)
        {
            return string.Concat(agg.GetType().Name, ":", agg.Id);
        }
    }
}
namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository;

    #endregion

    public class StreamNameFromAggregateRootGetter : IGetStreamNameFromAggregateRoot
    {
        #region Public Methods and Operators

        public string GetStreamName(AggregateRoot aggregateRoot)
        {
            return string.Concat(aggregateRoot.GetType().Name, "-", aggregateRoot.Id.ToString().Replace("-", "_"));
        }

        #endregion
    }
}
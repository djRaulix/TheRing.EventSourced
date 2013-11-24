namespace TheRing.EventSourced.Domain.Repository
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IGetStreamNameFromAggregateRoot
    {
        #region Public Methods and Operators

        string GetStreamName(AggregateRoot aggregateRoot);

        #endregion
    }
}
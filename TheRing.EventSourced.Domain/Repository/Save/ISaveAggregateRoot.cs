namespace TheRing.EventSourced.Domain.Repository.Save
{
    using TheRing.EventSourced.Domain.Aggregate;

    public interface ISaveAggregateRoot
    {
        #region Public Methods and Operators

        void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot;

        #endregion
    }
}
namespace TheRing.EventSourced.Domain.Repository
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IAggregateRootRepository
    {
        #region Public Methods and Operators

        TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot;

        TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot;

        void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot;

        #endregion
    }
}
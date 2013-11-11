namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IGetAggregateRoot
    {
        #region Public Methods and Operators

        TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot;

        #endregion
    }
}
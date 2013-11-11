namespace TheRing.EventSourced.Domain.Repository.Create
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface ICreateAggregateRoot
    {
        #region Public Methods and Operators

        TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot;

        #endregion
    }
}
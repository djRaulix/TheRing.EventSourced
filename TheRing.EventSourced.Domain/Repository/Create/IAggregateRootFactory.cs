namespace TheRing.EventSourced.Domain.Repository.Create
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IAggregateRootFactory
    {
        #region Public Methods and Operators

        T New<T>() where T : AggregateRoot;

        #endregion
    }
}
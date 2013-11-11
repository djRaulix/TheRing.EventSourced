namespace TheRing.EventSourced.Domain.Snapshot
{
    #region using

    using System;

    #endregion

    public interface IKeepSnapshot
    {
        #region Public Methods and Operators

        void Delete(Guid id);

        object Get(Guid id);

        void Set(Guid id, object snapshot);

        #endregion
    }
}
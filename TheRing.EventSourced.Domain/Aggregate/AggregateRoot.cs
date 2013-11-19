namespace TheRing.EventSourced.Domain.Aggregate
{
    #region using

    using System;
    using System.Collections.Generic;

    #endregion

    public abstract class AggregateRoot
    {
        #region Public Properties

        public Guid Id { get; internal set; }

        public abstract int Version { get; }

        #endregion

        #region Properties

        internal abstract IEnumerable<object> Changes { get; }

        internal abstract int SnapshotVersion { get; }

        internal abstract int OriginalVersion { get; }

        #endregion

        #region Methods

        internal abstract void LoadFromHistory(IEnumerable<object> history);

        internal abstract void TakeSnapshot();

        internal abstract bool RestoreSnapshot(object snapshot);

        #endregion
    }
}
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

        #endregion

        #region Methods

        internal abstract void LoadFromHistory(IEnumerable<object> history);

        internal abstract void RestoreFromSnapshot(object snapshot);

        internal abstract object TakeSnapshot();

        protected abstract void Apply(object @event);

        #endregion
    }
}
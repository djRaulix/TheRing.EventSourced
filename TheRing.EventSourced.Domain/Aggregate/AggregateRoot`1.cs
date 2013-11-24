namespace TheRing.EventSourced.Domain.Aggregate
{
    #region using

    using System.Collections.Generic;

    #endregion

    public abstract class AggregateRoot<TState> : AggregateRoot
        where TState : AggregateRootState, new()
    {
        #region Fields

        private readonly Queue<object> changes = new Queue<object>();

        private int originalVersion;

        private int snapshotVersion;

        private TState state = new TState();

        #endregion


        #region Public Properties

        public override sealed int Version
        {
            get
            {
                return this.state.Version;
            }
        }

        #endregion

        #region Properties

        internal override sealed IEnumerable<object> Changes
        {
            get
            {
                return this.changes;
            }
        }

        internal override int OriginalVersion
        {
            get
            {
                return this.originalVersion;
            }
        }

        internal override int SnapshotVersion
        {
            get
            {
                return this.snapshotVersion;
            }
        }

        protected TState State
        {
            get
            {
                return this.state;
            }
        }

        #endregion

        #region Methods

        internal override sealed void LoadFromHistory(IEnumerable<object> history)
        {
            foreach (var @event in history)
            {
                this.When(@event);
            }

            this.originalVersion = this.state.Version;
        }

        internal override sealed void RestoreSnapshot(object snapshot)
        {
            this.state = (TState)snapshot;
            this.originalVersion = this.snapshotVersion = this.state.Version;
        }

        internal override sealed object TakeSnapshot()
        {
            return this.state;
        }

        protected void Apply(object @event)
        {
            this.When(@event);
            this.changes.Enqueue(@event);
        }

        private void When(dynamic @event)
        {
            this.state.When(@event);
            this.state.Version++;
        }

        #endregion
    }
}
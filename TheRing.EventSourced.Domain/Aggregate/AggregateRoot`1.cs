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

        protected AggregateRoot()
        {
            this.state.Version = this.snapshotVersion = this.originalVersion = -1;
        }

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

        internal override sealed bool RestoreSnapshot(dynamic snapshot)
        {
            return this.TryRestoreSnapshot(snapshot);
        }

        internal override sealed void TakeSnapshot()
        {
            this.Apply(this.state);
        }

        protected void Apply(object @event)
        {
            this.When(@event);
            this.changes.Enqueue(@event);
        }

        private bool TryRestoreSnapshot(TState snapshot)
        {
            this.state = snapshot;
            this.originalVersion = this.snapshotVersion = ++this.state.Version;
            return true;
        }

        private bool TryRestoreSnapshot(object @event)
        {
            return false;
        }

        private void When(dynamic @event)
        {
            this.state.When(@event);
            this.state.Version++;
        }

        #endregion
    }
}
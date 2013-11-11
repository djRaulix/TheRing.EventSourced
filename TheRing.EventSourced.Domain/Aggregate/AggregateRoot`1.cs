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
                this.ApplyEvent(@event);
            }
        }

        internal override sealed void RestoreFromSnapshot(object snapshot)
        {
            var state = snapshot as TState;

            if (state != null)
            {
                this.state = state;
            }
        }

        internal override sealed object TakeSnapshot()
        {
            return this.state;
        }

        protected override sealed void Apply(object @event)
        {
            this.ApplyEvent(@event);
            this.changes.Enqueue(@event);
        }

        private void ApplyEvent(object @event)
        {
            this.state.When(@event);
            this.state.Version++;
        }

        #endregion
    }
}
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
            var enumerator = history.GetEnumerator();
            enumerator.MoveNext();
            this.Init((dynamic)enumerator.Current);
            while (enumerator.MoveNext())
            {
                this.When(enumerator.Current);
            }
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

        private void Init(TState snapshot)
        {
            this.state = snapshot;
            this.state.Version++;
        }

        private void Init(object @event)
        {
            this.When(@event);
        }

        private void When(dynamic @event)
        {
            this.state.When(@event);
            this.state.Version++;
        }

        #endregion
    }
}
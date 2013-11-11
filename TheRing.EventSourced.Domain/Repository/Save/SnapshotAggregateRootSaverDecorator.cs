namespace TheRing.EventSourced.Domain.Repository.Save
{
    #region using

    using System.Linq;

    using TheRing.EventSourced.Domain.Snapshot;

    #endregion

    public class SnapshotAggregateRootSaverDecorator : AggregateRootSaverDecorator
    {
        #region Constants

        private const int NbEventsBeforeSnapshot = 200;

        #endregion

        #region Fields

        private readonly IKeepSnapshot snapshotKeeper;

        #endregion

        #region Constructors and Destructors

        public SnapshotAggregateRootSaverDecorator(ISaveAggregateRoot decorated, IKeepSnapshot snapshotKeeper)
            : base(decorated)
        {
            this.snapshotKeeper = snapshotKeeper;
        }

        #endregion

        #region Public Methods and Operators

        public override void Save<TAgg>(TAgg aggregateRoot)
        {
            base.Save(aggregateRoot);

            var originalVersion = aggregateRoot.Version - aggregateRoot.Changes.Count();
            var originalModulo = originalVersion % NbEventsBeforeSnapshot;
            var modulo = aggregateRoot.Version % NbEventsBeforeSnapshot;
            if (originalModulo > modulo)
            {
                this.snapshotKeeper.Set(aggregateRoot.Id, aggregateRoot.TakeSnapshot());
            }
        }

        #endregion
    }
}
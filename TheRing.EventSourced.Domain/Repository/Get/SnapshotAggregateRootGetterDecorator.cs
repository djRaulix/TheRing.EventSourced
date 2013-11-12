namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Snapshot;

    #endregion

    public class SnapshotAggregateRootGetterDecorator : AggregateRootGetterDecorator
    {
        #region Fields

        private readonly IKeepSnapshot snapshotKeeper;

        #endregion

        #region Constructors and Destructors

        public SnapshotAggregateRootGetterDecorator(IGetAggregateRoot decorated, IKeepSnapshot snapshotKeeper)
            : base(decorated)
        {
            this.snapshotKeeper = snapshotKeeper;
        }

        #endregion

        #region Public Methods and Operators

        public override TAgg Get<TAgg>(Guid id)
        {
            var aggregateRoot = base.Get<TAgg>(id);

            aggregateRoot.RestoreFromSnapshot(this.snapshotKeeper.Get(aggregateRoot.Id));

            return aggregateRoot;
        }

        #endregion
    }
}
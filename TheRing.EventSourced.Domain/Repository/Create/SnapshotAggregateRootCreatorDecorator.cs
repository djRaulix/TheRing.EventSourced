namespace TheRing.EventSourced.Domain.Repository.Create
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Snapshot;

    #endregion

    public class SnapshotAggregateRootCreatorDecorator : AggregateRootCreatorDecorator
    {
        #region Fields

        private readonly IKeepSnapshot snapshotKeeper;

        #endregion

        #region Constructors and Destructors

        public SnapshotAggregateRootCreatorDecorator(ICreateAggregateRoot decorated, IKeepSnapshot snapshotKeeper)
            : base(decorated)
        {
            this.snapshotKeeper = snapshotKeeper;
        }

        #endregion

        #region Public Methods and Operators

        public override TAgg Create<TAgg>(Guid id)
        {
            var aggregateRoot = base.Create<TAgg>(id);

            aggregateRoot.RestoreFromSnapshot(this.snapshotKeeper.Get(aggregateRoot.Id));

            return aggregateRoot;
        }

        #endregion
    }
}
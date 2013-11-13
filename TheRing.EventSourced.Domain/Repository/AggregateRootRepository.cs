namespace TheRing.EventSourced.Domain.Repository
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository.Create;
    using TheRing.EventSourced.Domain.Repository.Get;
    using TheRing.EventSourced.Domain.Repository.Save;

    #endregion

    public class AggregateRootRepository : IAggregateRootRepository
    {
        #region Fields

        private readonly ICreateAggregateRoot creator;

        private readonly IGetAggregateRoot getter;

        private readonly ISaveAggregateRoot saver;

        #endregion

        #region Constructors and Destructors

        public AggregateRootRepository(ICreateAggregateRoot creator, IGetAggregateRoot getter, ISaveAggregateRoot saver)
        {
            this.creator = creator;
            this.getter = getter;
            this.saver = saver;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            return this.creator.Create<TAgg>(id);
        }

        public TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            return this.getter.Get<TAgg>(id);
        }

        public void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot
        {
            this.saver.Save(aggregateRoot);
        }

        #endregion
    }
}
namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository.Create;

    #endregion

    public class AggregateRootGetter : IGetAggregateRoot
    {
        #region Fields

        private readonly ICreateAggregateRoot aggregateRootCreator;

        #endregion

        #region Constructors and Destructors

        public AggregateRootGetter(ICreateAggregateRoot aggregateRootCreator)
        {
            this.aggregateRootCreator = aggregateRootCreator;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var aggregateRoot = this.aggregateRootCreator.Create<TAgg>(id);
            return aggregateRoot;
        }

        #endregion
    }
}
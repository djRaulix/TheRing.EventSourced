namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Exceptions;
    using TheRing.EventSourced.Domain.Repository.Create;

    #endregion

    public class AggregateRootGetter : IGetAggregateRoot
    {
        #region Fields

        private readonly ICreateAggregateRoot aggregateRootCreator;

        private readonly IGetEventsOnStream eventsGetter;

        private readonly IGetStreamNameFromAggregateRoot streamNameGetter;

        #endregion

        #region Constructors and Destructors

        public AggregateRootGetter(
            ICreateAggregateRoot aggregateRootCreator, 
            IGetEventsOnStream eventsGetter, 
            IGetStreamNameFromAggregateRoot streamNameGetter)
        {
            this.aggregateRootCreator = aggregateRootCreator;
            this.eventsGetter = eventsGetter;
            this.streamNameGetter = streamNameGetter;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var aggregateRoot = this.aggregateRootCreator.Create<TAgg>(id);

            aggregateRoot.LoadFromHistory(
                this.eventsGetter.Get(this.streamNameGetter.Get(aggregateRoot), aggregateRoot.Version + 1));

            if (aggregateRoot.Version == 0)
            {
                throw new UnKnownAggregateRootException(id);
            }

            return aggregateRoot;
        }

        #endregion
    }
}
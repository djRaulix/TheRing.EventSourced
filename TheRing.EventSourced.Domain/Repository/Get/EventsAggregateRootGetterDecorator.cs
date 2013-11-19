namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.Domain.Exceptions;

    using Thering.EventSourced.Eventing;

    #endregion

    public class EventsAggregateRootGetterDecorator : AggregateRootGetterDecorator
    {
        #region Fields

        private readonly IGetEventsOnStream eventsGetter;

        private readonly IGetStreamNameFromAggregateRoot streamNameGetter;

        #endregion

        #region Constructors and Destructors

        public EventsAggregateRootGetterDecorator(
            IGetAggregateRoot decorated, 
            IGetEventsOnStream eventsGetter, 
            IGetStreamNameFromAggregateRoot streamNameGetter)
            : base(decorated)
        {
            this.eventsGetter = eventsGetter;
            this.streamNameGetter = streamNameGetter;
        }

        #endregion

        #region Public Methods and Operators

        public override TAgg Get<TAgg>(Guid id)
        {
            var aggregateRoot = base.Get<TAgg>(id);

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
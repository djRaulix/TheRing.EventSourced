namespace TheRing.EventSourced.Domain.Repository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Exceptions;
    using TheRing.EventSourced.Domain.Properties;
    using TheRing.EventSourced.Domain.Repository.Factory;

    using Thering.EventSourced.Eventing;

    #endregion

    public class AggregateRootRepository : IAggregateRootRepository
    {
        #region Fields

        private readonly IAggregateRootFactory aggregateRootFactory;

        private readonly IGetEventsOnStream eventsGetter;

        private readonly ISAveEventsOnStream eventsSaver;

        private readonly IGetStreamNameFromAggregateRoot streamNameGetter;

        #endregion

        #region Constructors and Destructors

        public AggregateRootRepository(
            IAggregateRootFactory aggregateRootFactory, 
            IGetEventsOnStream eventsGetter, 
            ISAveEventsOnStream eventsSaver, 
            IGetStreamNameFromAggregateRoot streamNameGetter)
        {
            this.aggregateRootFactory = aggregateRootFactory;
            this.eventsGetter = eventsGetter;
            this.eventsSaver = eventsSaver;
            this.streamNameGetter = streamNameGetter;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var agg = this.aggregateRootFactory.Create<TAgg>();
            agg.Id = id;
            return agg;
        }

        public TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var agg = this.aggregateRootFactory.Create<TAgg>();
            agg.Id = id;
            var events = this.eventsGetter.GetBackward(this.streamNameGetter.Get(agg));

            agg.LoadFromHistory(this.GetOrderedEvents(events.GetEnumerator(), agg.RestoreSnapshot));

            if (agg.Version == 0)
            {
                throw new UnKnownAggregateRootException(id);
            }

            return agg;
        }

        public void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot
        {
            if (aggregateRoot.Version - aggregateRoot.SnapshotVersion >= Settings.Default.NbEventsBeforeSnapshot)
            {
                aggregateRoot.TakeSnapshot();
            }

            this.eventsSaver.Save(
                this.streamNameGetter.Get(aggregateRoot), 
                aggregateRoot.Changes, 
                expectedVersion: aggregateRoot.OriginalVersion);
        }

        #endregion

        #region Methods

        private IEnumerable<object> GetOrderedEvents(IEnumerator<object> enumerator, Func<object, bool> restoreSnapshot)
        {
            while (enumerator.MoveNext() && !restoreSnapshot(enumerator.Current))
            {
                foreach (var @event in this.GetOrderedEvents(enumerator, restoreSnapshot))
                {
                    yield return @event;
                }

                yield return enumerator.Current;
            }
        }

        #endregion
    }
}
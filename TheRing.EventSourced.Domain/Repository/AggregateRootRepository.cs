namespace TheRing.EventSourced.Domain.Repository
{
    #region using

    using System;
    using System.Linq;

    using TheRing.EventSourced.Core.Log;
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Exceptions;
    using TheRing.EventSourced.Domain.Properties;
    using TheRing.EventSourced.Domain.Repository.Factory;
    using TheRing.EventSourced.Domain.Repository.Snapshot;

    using Thering.EventSourced.Eventing;

    #endregion

    public class AggregateRootRepository : IAggregateRootRepository
    {
        #region Fields

        private readonly IAggregateRootFactory aggregateRootFactory;

        private readonly IEventStreamRepository eventRepository;

        private readonly ILogger log = LogManager.GetLoggerFor<AggregateRootRepository>();

        private readonly IGetStreamNameFromAggregateRoot streamNameGetter;

        private readonly IGetSnapshotKeyFromAggregateRoot snapshotKeyGetter;

        private readonly ISnapshoter snapshoter;

        #endregion

        #region Constructors and Destructors

        public AggregateRootRepository(
            IAggregateRootFactory aggregateRootFactory, 
            IEventStreamRepository eventRepository, 
            IGetStreamNameFromAggregateRoot streamNameGetter,
            IGetSnapshotKeyFromAggregateRoot snapshotKeyGetter,
            ISnapshoter snapshoter)
        {
            this.aggregateRootFactory = aggregateRootFactory;
            this.eventRepository = eventRepository;
            this.streamNameGetter = streamNameGetter;
            this.snapshotKeyGetter = snapshotKeyGetter;
            this.snapshoter = snapshoter;
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

            try
            {
                var snapshot =
                    this.snapshoter.Get(this.snapshotKeyGetter.GetKey(agg));
                if (snapshot != null)
                {
                    agg.RestoreSnapshot(snapshot);
                }
            }
            catch (Exception ex)
            {
                this.log.ErrorException(
                    ex, 
                    "Unable to restore snapshot {0} agg with {1} id", 
                    agg.GetType().Name, 
                    agg.Id);
            }

            agg.LoadFromHistory(this.eventRepository.Get(this.streamNameGetter.GetStreamName(agg), agg.Version));

            if (agg.Version == 0)
            {
                throw new UnKnownAggregateRootException(id);
            }

            return agg;
        }

        public void Save<TAgg>(TAgg agg) where TAgg : AggregateRoot
        {
            this.eventRepository.Save(
                this.streamNameGetter.GetStreamName(agg), 
                agg.Changes, 
                expectedVersion: agg.OriginalVersion - 1);

            try
            {
                if (agg.Version - agg.SnapshotVersion >= Settings.Default.NbEventsBeforeSnapshot)
                {
                    this.snapshoter.Set(this.snapshotKeyGetter.GetKey(agg), agg.TakeSnapshot());
                }
            }
            catch (Exception ex)
            {
                this.log.ErrorException(ex, "Unable to snapshot {0} agg with {1} id", agg.GetType().Name, agg.Id);
            }
        }

        #endregion
    }
}
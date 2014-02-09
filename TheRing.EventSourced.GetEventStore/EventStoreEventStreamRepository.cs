using EventStore.ClientAPI.SystemData;

namespace TheRing.EventSourced.GetEventStore
{
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using Thering.EventSourced.Eventing.Events;
    using Thering.EventSourced.Eventing.Repositories;

    using StreamPosition = Thering.EventSourced.Eventing.Constants.StreamPosition;

    public class EventStoreEventStreamRepository : IEventStreamRepository
    {
        #region Fields

        private readonly IEventStoreConnection eventStoreConnection;

        private readonly ISerializeEvent serializer;

        #endregion

        #region Constructors and Destructors

        public EventStoreEventStreamRepository(IEventStoreConnection eventStoreConnection, ISerializeEvent serializer)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.serializer = serializer;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<object> Get(
            string streamName, 
            int fromVersion = StreamPosition.Start, 
            int count = int.MaxValue)
        {
            return this.GetWithMetadata(streamName, fromVersion, count).Select(e => e.Event);
        }

        public IEnumerable<object> GetBackward(
            string streamName, 
            int fromVersion = StreamPosition.End, 
            int count = int.MaxValue)
        {
            return this.GetBackwardWithMetadata(streamName, fromVersion, count).Select(e => e.Event);
        }

        public IEnumerable<EventWithMetadata> GetBackwardWithMetadata(
            string streamName, 
            int fromVersion = StreamPosition.End, 
            int count = int.MaxValue)
        {
            var currentSlice = this.eventStoreConnection.ReadStreamEventsBackward(streamName, fromVersion, count, false);

            return currentSlice.Events.Select(evnt => this.serializer.Deserialize(evnt.OriginalEvent));
        }

        public IEnumerable<EventWithMetadata> GetWithMetadata(
            string streamName, 
            int fromVersion = StreamPosition.Start, 
            int count = int.MaxValue)
        {
            var currentSlice = this.eventStoreConnection.ReadStreamEventsForward(streamName, fromVersion, count, false);

            return currentSlice.Events.Select(evnt => this.serializer.Deserialize(evnt.OriginalEvent));
        }

        public void Save(
            string streamName, 
            IEnumerable<object> events, 
            IDictionary<string, object> headers = null, 
            int? expectedVersion = null)
        {
            var expectVersion = expectedVersion ?? ExpectedVersion.Any;

            var eventsToSave = events.Select(e => this.serializer.Serialize(e, headers));

            this.eventStoreConnection.AppendToStream(streamName, expectVersion, eventsToSave, new UserCredentials("admin", "admin"));
        }

        public void Save(
            string streamName, 
            object @event, 
            IDictionary<string, object> headers = null, 
            int? expectedVersion = null)
        {
            this.Save(streamName, new[] { @event }, headers, expectedVersion);
        }

        #endregion
    }
}
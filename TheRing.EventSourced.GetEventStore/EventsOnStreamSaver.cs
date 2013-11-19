namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;

    #endregion

    public class EventsOnStreamSaver : ISAveEventsOnStream
    {
        #region Constants

        private const int WritePageSize = 500;

        #endregion

        #region Fields

        private readonly ITransformToEventData eventDataTransformer;

        private readonly IEventStoreConnection eventStoreConnection;

        #endregion

        #region Constructors and Destructors

        public EventsOnStreamSaver(
            IEventStoreConnection eventStoreConnection, 
            ITransformToEventData eventDataTransformer)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.eventDataTransformer = eventDataTransformer;
        }

        #endregion

        #region Public Methods and Operators

        public void Save(
            string streamName, 
            IEnumerable<object> events, 
            IDictionary<string, object> headers = null, 
            int? expectedVersion = null)
        {
            var expectVersion = expectedVersion ?? ExpectedVersion.Any;

            var eventsToSave = events.Select(e => this.eventDataTransformer.Transform(e, headers)).ToList();

            var nbEvents = eventsToSave.Count();

            if (nbEvents < WritePageSize)
            {
                this.eventStoreConnection.AppendToStream(streamName, expectVersion, eventsToSave);
            }
            else
            {
                var transaction = this.eventStoreConnection.StartTransaction(streamName, expectVersion);

                var position = 0;
                while (position < nbEvents)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    transaction.Write(pageEvents);
                    position += WritePageSize;
                }

                transaction.Commit();
            }
        }

        #endregion
    }
}
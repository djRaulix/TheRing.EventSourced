namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System.Collections.Generic;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.GetEventStore.Exceptions;

    #endregion

    public class EventsOnStreamGetter : IGetEventsOnStream
    {
        #region Constants

        private const int ReadPageSize = 500;

        #endregion

        #region Fields

        private readonly IEventStoreConnection eventStoreConnection;

        private readonly IGetEventFromRecorded fromRecordedEventGetter;

        #endregion

        #region Constructors and Destructors

        public EventsOnStreamGetter(
            IEventStoreConnection eventStoreConnection, 
            IGetEventFromRecorded fromRecordedEventGetter)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.fromRecordedEventGetter = fromRecordedEventGetter;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<object> Get(string streamName, int fromVersion = 0, int toVersion = int.MaxValue)
        {
            var sliceCount = this.getSliceCount(fromVersion, toVersion);
            var endOfStream = false;

            while (!(sliceCount <= 0 || endOfStream))
            {
                var currentSlice = this.eventStoreConnection.ReadStreamEventsForward(
                    streamName, 
                    fromVersion, 
                    sliceCount, 
                    false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                {
                    throw new StreamNotFoundException(streamName);
                }

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                {
                    throw new StreamDeletedException(streamName);
                }

                foreach (var evnt in currentSlice.Events)
                {
                    yield return this.fromRecordedEventGetter.Get(evnt.OriginalEvent);
                }

                endOfStream = currentSlice.IsEndOfStream;

                sliceCount = this.getSliceCount(currentSlice.NextEventNumber, toVersion);
            }
        }

        #endregion

        #region Methods

        private int getSliceCount(int fromVersion, int toVersion)
        {
            return fromVersion + ReadPageSize <= toVersion ? ReadPageSize : toVersion - fromVersion;
        }

        #endregion
    }
}
namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using Thering.EventSourced.Eventing;

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

        public IEnumerable<object> Get(string streamName, int? fromVersion = null, int? toVersion = null)
        {
            return this.GetWithMetadata(streamName, fromVersion, toVersion).Select(e => e.Event);
        }

        public IEnumerable<object> GetBackward(string streamName, int? fromVersion = null, int? toVersion = null)
        {
            return this.GetBackwardWithMetadata(streamName, fromVersion, toVersion).Select(e => e.Event);
        }

        public IEnumerable<EventWithMetadata> GetBackwardWithMetadata(
            string streamName, 
            int? fromVersion = null, 
            int? toVersion = null)
        {
            var from = fromVersion ?? StreamPosition.End;
            var to = toVersion ?? StreamPosition.Start;

            return this.Get(
                streamName, 
                (start, count) => this.eventStoreConnection.ReadStreamEventsBackward(streamName, start, count, false), 
                this.GetSliceCountBackward, 
                from, 
                to);
        }

        public IEnumerable<EventWithMetadata> GetWithMetadata(
            string streamName, 
            int? fromVersion = null, 
            int? toVersion = null)
        {
            var from = fromVersion ?? StreamPosition.Start;
            var to = toVersion ?? StreamPosition.End;
            return this.Get(
                streamName, 
                (start, count) => this.eventStoreConnection.ReadStreamEventsForward(streamName, start, count, false), 
                this.GetSliceCount, 
                from, 
                to);
        }

        #endregion

        #region Methods

        private IEnumerable<EventWithMetadata> Get(
            string streamName, 
            Func<int, int, StreamEventsSlice> read, 
            Func<int, int, int> getSliceCnt, 
            int fromVersion, 
            int toVersion)
        {
            var sliceCount = getSliceCnt(fromVersion, toVersion);
            var endOfStream = false;

            while (!(sliceCount <= 0 || endOfStream))
            {
                var currentSlice = read(fromVersion, sliceCount);

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

                sliceCount = this.GetSliceCount(currentSlice.NextEventNumber, toVersion);
            }
        }

        private int GetSliceCount(int fromVersion, int toVersion)
        {
            return fromVersion + ReadPageSize <= toVersion ? ReadPageSize : toVersion - fromVersion;
        }

        private int GetSliceCountBackward(int fromVersion, int toVersion)
        {
            return fromVersion - ReadPageSize >= toVersion ? ReadPageSize : fromVersion - toVersion;
        }

        #endregion
    }
}
namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System;
    using System.Collections.Generic;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;

    using TheRing.EventSourced.Core;

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

        public IEnumerable<object> Get(string streamName, int fromVersion, int toVersion, bool backward = false)
        {
            Func<int,int,StreamEventsSlice> read;
            Func<int, int, int> getSliceCnt;
            if (backward)
            {
                read = (start, count) => this.eventStoreConnection.ReadStreamEventsBackward(streamName,start,count, false);
                getSliceCnt = this.getSliceCount;
            }
            else
            {
                read = (start, count) => this.eventStoreConnection.ReadStreamEventsForward(streamName, start, count, false);
                getSliceCnt = this.getSliceCount;
            }
            
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
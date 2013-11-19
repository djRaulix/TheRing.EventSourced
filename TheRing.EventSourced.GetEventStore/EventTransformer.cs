namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System;
    using System.Collections.Generic;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;

    using Thering.EventSourced.Eventing;

    #endregion

    public class EventTransformer : ITransformToEventData, IGetEventFromRecorded
    {
        #region Constants

        private const string EventClrTypeHeader = "EventClrTypeName";

        #endregion

        #region Fields

        private readonly ISerialize serializer;

        #endregion

        #region Constructors and Destructors

        public EventTransformer(ISerialize serializer)
        {
            this.serializer = serializer;
        }

        #endregion

        #region Public Methods and Operators

        public EventWithMetadata Get(RecordedEvent recordedEvent)
        {
            var eventHeaders = this.serializer.Deserialize<Dictionary<string, object>>(recordedEvent.Metadata);
            var @event = this.serializer.Deserialize(
                recordedEvent.Data, 
                Type.GetType((string)eventHeaders[EventClrTypeHeader]));

            return new EventWithMetadata(@event, eventHeaders);
        }

        public EventData Transform(object @event, IDictionary<string, object> headers = null)
        {
            var data = this.serializer.Serialize(@event);

            var eventHeaders = headers == null
                                   ? new Dictionary<string, object>()
                                   : new Dictionary<string, object>(headers);

            eventHeaders.Add(EventClrTypeHeader, @event.GetType().AssemblyQualifiedName);

            var metadata = this.serializer.Serialize(eventHeaders);
            var typeName = @event.GetType().Name;

            return new EventData(Guid.NewGuid(), typeName, true, data, metadata);
        }

        #endregion
    }
}
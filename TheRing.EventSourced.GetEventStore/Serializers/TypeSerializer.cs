namespace TheRing.EventSourced.GetEventStore.Serializers
{
    using System;
    using System.Collections.Generic;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;

    using Thering.EventSourced.Eventing.Events;

    public class TypeSerializer : ISerializeEvent
    {
        #region Fields

        private readonly ISerialize serializer;

        #endregion

        #region Constructors and Destructors

        public TypeSerializer(ISerialize serializer)
        {
            this.serializer = serializer;
        }

        #endregion

        #region Public Methods and Operators

        public EventWithMetadata Deserialize(RecordedEvent recordedEvent)
        {
            var eventHeaders = this.serializer.Deserialize<Dictionary<string, object>>(recordedEvent.Metadata);
            var @event = this.serializer.Deserialize(
                recordedEvent.Data,
                typeof(Type));

            return new EventWithMetadata(@event, eventHeaders);
        }

        public EventData Serialize(object @event, IDictionary<string, object> headers = null)
        {
            var data = this.serializer.Serialize(@event);

            var eventHeaders = headers == null ? null : new Dictionary<string, object>(headers);

            var metadata = this.serializer.Serialize(eventHeaders);

            return new EventData(Guid.NewGuid(), typeof(Type).AssemblyQualifiedName, true, data, metadata);
        }

        #endregion
    }
}
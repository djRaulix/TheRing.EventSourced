namespace TheRing.EventSourced.GetEventStore.Serializers
{
    #region using

    using System;
    using System.Collections.Generic;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;

    using Thering.EventSourced.Eventing.Aliaser;
    using Thering.EventSourced.Eventing.Events;

    #endregion

    public class EventSerializer : ISerializeEvent
    {
        #region Fields

        private readonly ISerialize serializer;

        private readonly ITypeAliaser typeAliaser;

        #endregion

        #region Constructors and Destructors

        public EventSerializer(ISerialize serializer, ITypeAliaser typeAliaser)
        {
            this.serializer = serializer;
            this.typeAliaser = typeAliaser;
        }

        #endregion

        #region Public Methods and Operators

        public EventWithMetadata Deserialize(RecordedEvent recordedEvent)
        {
            var eventHeaders = this.serializer.Deserialize<Dictionary<string, object>>(recordedEvent.Metadata);
            var @event = this.serializer.Deserialize(
                recordedEvent.Data, 
                this.typeAliaser.GetType(recordedEvent.EventType));

            return new EventWithMetadata(@event, eventHeaders);
        }

        public EventData Serialize(object @event, IDictionary<string, object> headers = null)
        {
            var data = this.serializer.Serialize(@event);

            var eventHeaders = headers == null ? null : new Dictionary<string, object>(headers);

            var metadata = this.serializer.Serialize(eventHeaders);

            return new EventData(Guid.NewGuid(), this.typeAliaser.GetAlias(@event), true, data, metadata);
        }

        #endregion
    }
}
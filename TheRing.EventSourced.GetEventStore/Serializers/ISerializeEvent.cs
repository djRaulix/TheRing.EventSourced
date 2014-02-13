namespace TheRing.EventSourced.GetEventStore.Serializers
{
    using System.Collections.Generic;

    using EventStore.ClientAPI;

    using Thering.EventSourced.Eventing.Events;

    public interface ISerializeEvent
    {
        #region Public Methods and Operators

        EventWithMetadata Deserialize(RecordedEvent recordedEvent);

        EventData Serialize(object @event, IDictionary<string, object> headers = null);

        #endregion
    }
}
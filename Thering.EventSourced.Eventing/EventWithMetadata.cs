namespace Thering.EventSourced.Eventing
{
    using System;
    using System.Collections.Generic;

    public class EventWithMetadata
    {
        public readonly object Event;

        public readonly int EventPosition;

        public readonly Type EventType;

        public readonly IDictionary<string, object> Metadata;

        public EventWithMetadata(object @event,int eventPosition, IDictionary<string, object> metadata = null)
        {
            this.Metadata = metadata;
            this.EventPosition = eventPosition;
            this.EventType = @event.GetType();
            this.Event = @event;
        }
    }
}
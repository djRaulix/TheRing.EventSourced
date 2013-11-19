namespace Thering.EventSourced.Eventing
{
    using System.Collections.Generic;

    public class EventWithMetadata
    {
        public readonly object Event;

        public readonly IDictionary<string, object> Metadata;

        public EventWithMetadata(object @event, IDictionary<string, object> metadata)
        {
            this.Metadata = metadata;
            this.Event = @event;
        }
    }
}
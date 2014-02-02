namespace Thering.EventSourced.Eventing.Events
{
    using System.Collections.Generic;

    public class EventWithMetadata
    {
        public readonly object Event;

        public readonly IDictionary<string, object> Metadata;

        public EventWithMetadata(object @event, IDictionary<string, object> metadata = null)
        {
            this.Metadata = metadata;
            this.Event = @event;
        }
    }
}
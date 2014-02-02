namespace Thering.EventSourced.Eventing
{
    using System;

    public class AggregateEventHandled
    {
        public string EventHandlerFullTypeName { get; set; }

        public int Position { get; set; }
    }
}
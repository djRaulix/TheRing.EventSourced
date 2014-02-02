namespace Thering.EventSourced.Eventing.Events
{
    public class AggregateEventHandled
    {
        public string EventHandlerFullTypeName { get; set; }

        public int Position { get; set; }
    }
}
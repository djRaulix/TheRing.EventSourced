namespace Thering.EventSourced.Eventing.Handlers
{
    using Thering.EventSourced.Eventing.Events;

    public class EventHandlerQueue : AbstractEventQueue
    {
        private readonly IHandleEvent eventHandler;

        public EventHandlerQueue(IHandleEvent eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        protected override void HandleEvent(EventWithMetadata @event, int eventPosition)
        {
            this.eventHandler.Handle(@event, eventPosition);
        }
    }
}
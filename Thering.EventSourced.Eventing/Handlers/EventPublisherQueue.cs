namespace Thering.EventSourced.Eventing.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Thering.EventSourced.Eventing.Events;
    using Thering.EventSourced.Eventing.Repositories;

    public class EventPublisherQueue : AbstractEventQueue
    {
        private readonly Func<Type, IEnumerable<IEventQueue>> eventQueueFactory;
        private readonly IEventPositionManager eventPositionManager;

        public EventPublisherQueue(Func<Type, IEnumerable<IEventQueue>> eventQueueFactory, IEventPositionManager eventPositionManager)
        {
            this.eventQueueFactory = eventQueueFactory;
            this.eventPositionManager = eventPositionManager;
        }

        protected override void BeforePushing(EventWithMetadata @event, int eventPosition)
        {
            this.eventPositionManager.Create(eventPosition, this.eventQueueFactory(@event.Event.GetType()).Count());
        }

        protected override void HandleEvent(EventWithMetadata @event, int eventPosition)
        {
            foreach (var eventQueue in this.eventQueueFactory(@event.Event.GetType()))
            {
                eventQueue.Push(@event, @eventPosition);
            }
        }
    }
}
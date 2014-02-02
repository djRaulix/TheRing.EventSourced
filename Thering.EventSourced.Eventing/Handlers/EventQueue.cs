namespace Thering.EventSourced.Eventing.Handlers
{
    using System.Collections.Concurrent;
    using System.Threading;

    using Thering.EventSourced.Eventing.Events;

    public class EventQueue : IEventQueue
    {
        private readonly IHandleEvent eventHandler;

        private readonly BlockingCollection<EventWithPosition> queue;


        public EventQueue(IHandleEvent eventHandler)
        {
            this.eventHandler = eventHandler;
            this.queue = new BlockingCollection<EventWithPosition>();
            var waiter = new Thread(this.WaitAndHandle);
            waiter.Start();
        }

        public void Push(EventWithMetadata @event, int position)
        {
            if (!this.queue.IsAddingCompleted)
            {
                this.queue.Add(new EventWithPosition(@event, position));    
            }
        }

        public void Stop()
        {
            this.queue.CompleteAdding();
        }

        private void WaitAndHandle()
        {
            while (!this.queue.IsCompleted)
            {
                var eventWithPosition = this.queue.Take();
                this.eventHandler.Handle(eventWithPosition.Event, eventWithPosition.Position);
            }
        }

        private class EventWithPosition
        {
            public readonly EventWithMetadata Event;

            public readonly int Position;

            public EventWithPosition(EventWithMetadata @event, int position)
            {
                this.Event = @event;
                this.Position = position;
            }
        }
    }
}
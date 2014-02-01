namespace Thering.EventSourced.Eventing
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class EventQueue : IEventQueue
    {
        private readonly IHandleEvent eventHandler;

        private readonly BlockingCollection<EventWithPosition> queue;


        public EventQueue(IHandleEvent eventHandler)
        {
            this.eventHandler = eventHandler;
            this.queue = new BlockingCollection<EventWithPosition>();
            var waiter = new Thread(WaitAndHandle);
            waiter.Start();
        }

        public void Push(EventWithMetadata @event, int position)
        {
            if (!queue.IsAddingCompleted)
            {
                queue.Add(new EventWithPosition(@event, position));    
            }
        }

        public void Stop()
        {
            queue.CompleteAdding();
        }

        private void WaitAndHandle()
        {
            while (!queue.IsCompleted)
            {
                var eventWithPosition = queue.Take();
                eventHandler.Handle(eventWithPosition.Event, eventWithPosition.Position);
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
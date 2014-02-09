namespace Thering.EventSourced.Eventing.Handlers
{
    using System.Collections.Concurrent;
    using System.Threading;

    using Thering.EventSourced.Eventing.Events;

    public abstract class AbstractEventQueue : IEventQueue
    {
        private readonly BlockingCollection<EventWithPosition> queue;

        protected AbstractEventQueue()
        {
            this.queue = new BlockingCollection<EventWithPosition>();
            var waiter = new Thread(this.WaitAndHandle);
            waiter.Start();
        }

        public void Push(EventWithMetadata @event, int position)
        {
            if (!this.queue.IsAddingCompleted)
            {
                this.BeforePushing(@event, position);
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
                this.HandleEvent(eventWithPosition.Event, eventWithPosition.Position);
            }
        }

        protected virtual void BeforePushing(EventWithMetadata @event, int position)
        {
        }

        protected abstract void HandleEvent(EventWithMetadata @event, int eventPosition);

        private class EventWithPosition
        {
            internal readonly EventWithMetadata Event;

            internal readonly int Position;

            public EventWithPosition(EventWithMetadata @event, int position)
            {
                this.Event = @event;
                this.Position = position;
            }
        }
    }
}
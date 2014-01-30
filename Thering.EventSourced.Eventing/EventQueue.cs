namespace Thering.EventSourced.Eventing
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class EventQueue : IEventQueue
    {
        private readonly IEventHandler eventHandler;

        private readonly BlockingCollection<EventWithMetadata> queue;


        public EventQueue(IEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            this.queue = new BlockingCollection<EventWithMetadata>();
            var waiter = new Thread(WaitAndHandle);
            waiter.Start();
        }

        public void Push(EventWithMetadata @event)
        {
            if (!queue.IsAddingCompleted)
            {
                queue.Add(@event);    
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
                eventHandler.Handle(queue.Take());
            }
        }
    }
}
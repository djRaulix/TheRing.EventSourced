namespace Thering.EventSourced.Eventing
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class EventQueue<TEventHandler> : IEventQueue
    {
        private readonly TEventHandler eventHandler;

        private readonly BlockingCollection<object> queue;

        private bool stopped;

        public EventQueue(TEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            this.queue = new BlockingCollection<object>();
            var waiter = new Thread(WaitAndHandle);
            waiter.Start();
        }

        public void Push(object @event)
        {
            queue.Add(@event);
        }

        public void Stop()
        {
            queue.CompleteAdding();
            while (!queue.IsCompleted); //wait for empty
            stopped = true;
        }

        private void WaitAndHandle()
        {
            while (true)
            {
                HandleEvent((dynamic)queue.Take());
                if (stopped)
                {
                    break;
                }
            }
        }

        private void HandleEvent<T>(T @event)
        {
            ((IHandlesEvent<T>)eventHandler).Handles(@event);
        }
    }
}
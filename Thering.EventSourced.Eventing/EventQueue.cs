namespace Thering.EventSourced.Eventing
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class EventQueue : IEventQueue
    {
        private readonly IHandleEvent eventHandler;
        private readonly IHandleError errorHandler;

        private readonly BlockingCollection<object> queue;

        private bool stopped;

        public EventQueue(IHandleEvent eventHandler, IHandleError errorHandler)
        {
            this.eventHandler = eventHandler;
            this.errorHandler = errorHandler;
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
            try
            {
                ((IHandleEvent<T>)eventHandler).Handle(@event);
            }
            catch (Exception e)
            {
                errorHandler.HandleError(@event, e);
            }
        }
    }
}
namespace Thering.EventSourced.Eventing
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Threading;

    public class EventQueue : IEventQueue
    {
        private readonly IHandleEvent eventHandler;
        private readonly IHandleError errorHandler;

        private readonly BlockingCollection<object> queue;


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
            if (!queue.IsAddingCompleted)
            {
                Trace.WriteLine(DateTime.Now + " Push to queue !");
                queue.Add(@event);    
            }
        }

        public void Stop()
        {
            queue.CompleteAdding();
            Trace.WriteLine(DateTime.Now  + " Stop queue !");
        }

        private void WaitAndHandle()
        {
            while (!queue.IsCompleted)
            {
                HandleEvent((dynamic)queue.Take());
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
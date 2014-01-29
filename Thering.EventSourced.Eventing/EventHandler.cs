namespace Thering.EventSourced.Eventing
{
    using System;

    public class EventHandler : IEventHandler
    {
        private readonly object eventHandler;
        private readonly IHandleError errorHandler;

        public EventHandler(IHandleEvent eventHandler, IHandleError errorHandler)
        {
            this.eventHandler = eventHandler;
            this.errorHandler = errorHandler;
        }

        public void Handle(object @event)
        {
            HandleEvent((dynamic)@event);
        }

        private void HandleEvent<T>(T @event)
        {
            try
            {
                ((IHandleEvent<T>)this.eventHandler).Handle(@event);
            }
            catch (Exception e)
            {
                this.errorHandler.HandleError(@event, e);
            }
        }
    }
}
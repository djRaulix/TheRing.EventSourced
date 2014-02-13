namespace Thering.EventSourced.Eventing.Handlers
{
    using System;

    using Thering.EventSourced.Eventing.Events;
    using Thering.EventSourced.Eventing.Repositories;

    public class EventHandler : IHandleEvent
    {
        private readonly object eventHandler;
        private readonly IHandleError errorHandler;
        private readonly IEventPositionManager eventPositionManager;

        public EventHandler(object eventHandler, IHandleError errorHandler, IEventPositionManager eventPositionManager)
        {
            this.eventHandler = eventHandler;
            this.errorHandler = errorHandler;
            this.eventPositionManager = eventPositionManager;
        }

        public void Handle(EventWithMetadata eventWithMetadata, int positon)
        {
            var @event = eventWithMetadata.Event;
            HandleEvent((dynamic)@event, positon);
        }

        private void HandleEvent<T>(T @event, int eventPosition)
        {
            try
            {
                ((IHandleEvent<T>)this.eventHandler).Handle(@event);
            }
            catch (Exception e)
            {
                this.errorHandler.HandleError(@event, eventPosition, this.eventHandler.GetType(), e);
            }
            finally
            {
                this.eventPositionManager.Decrement(eventPosition);
            }
        }
    }
}
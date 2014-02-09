namespace Thering.EventSourced.Eventing.Handlers
{
    using System;

    using Thering.EventSourced.Eventing.Events;
    using Thering.EventSourced.Eventing.Repositories;

    public class EventHandler : IHandleEvent
    {
        private readonly object eventHandler;
        private readonly IHandleError errorHandler;
        private readonly IEventPositionRepository eventPositionRepository;

        public EventHandler(object eventHandler, IHandleError errorHandler, IEventPositionRepository eventPositionRepository)
        {
            this.eventHandler = eventHandler;
            this.errorHandler = errorHandler;
            this.eventPositionRepository = eventPositionRepository;
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
                this.errorHandler.HandleError(@event, e);
            }
            finally
            {
                this.eventPositionRepository.Decrement(eventPosition);
            }
        }
    }
}
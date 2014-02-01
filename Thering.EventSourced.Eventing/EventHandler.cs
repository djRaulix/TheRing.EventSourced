namespace Thering.EventSourced.Eventing
{
    using System;

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
                eventPositionRepository.Save(eventHandler.GetType(), typeof(T), eventPosition);
            }
            catch (Exception e)
            {
                this.errorHandler.HandleError(@event, e);
            }
        }
    }
}
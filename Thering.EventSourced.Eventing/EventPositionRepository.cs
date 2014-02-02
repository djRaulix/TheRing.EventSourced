namespace Thering.EventSourced.Eventing
{
    using System;

    public class EventPositionRepository : IEventPositionRepository
    {
        private readonly IEventStreamRepository eventStreamRepository;

        public EventPositionRepository(IEventStreamRepository eventStreamRepository)
        {
            this.eventStreamRepository = eventStreamRepository;
        }

        public void Save(Type eventHandlerType, int position)
        {
            var aggregateEventHandled = new AggregateEventHandled
            {
                EventHandlerFullTypeName = eventHandlerType.FullName, 
                Position = position
            };

            eventStreamRepository.Save(StreamId.EventPositionStream, aggregateEventHandled);
        }
    }
}
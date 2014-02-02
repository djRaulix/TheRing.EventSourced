namespace Thering.EventSourced.Eventing.Repositories
{
    using System;

    using Thering.EventSourced.Eventing.Constants;
    using Thering.EventSourced.Eventing.Events;

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

            this.eventStreamRepository.Save(StreamId.EventPositionStream, aggregateEventHandled);
        }
    }
}
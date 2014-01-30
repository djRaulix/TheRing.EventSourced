namespace WebSample.Eventing
{
    using System;

    using Thering.EventSourced.Eventing;

    public class EventPositionRepository : IEventPositionRepository
    {
        public void Save(Type eventHandlerType, Type eventType, int position)
        {
            
        }
    }
}
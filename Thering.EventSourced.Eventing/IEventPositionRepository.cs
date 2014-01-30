namespace Thering.EventSourced.Eventing
{
    using System;

    public interface IEventPositionRepository
    {
        void Save(Type eventHandlerType, Type eventType, int position);
    }
}
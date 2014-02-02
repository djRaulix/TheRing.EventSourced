namespace Thering.EventSourced.Eventing.Repositories
{
    using System;

    public interface IEventPositionRepository
    {
        void Save(Type eventHandlerType, int position);
    }
}
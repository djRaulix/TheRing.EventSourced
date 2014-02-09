namespace Thering.EventSourced.Eventing.Repositories
{
    using System;

    public interface IEventPositionRepository
    {
        void Create(int eventPosition, int eventHandlerNumber);

        void Decrement(int eventPosition);

        int? GetMinUnhandledPosition();
    }
}
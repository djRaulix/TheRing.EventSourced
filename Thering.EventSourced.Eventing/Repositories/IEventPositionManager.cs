namespace Thering.EventSourced.Eventing.Repositories
{
    public interface IEventPositionManager
    {
        void Create(int eventPosition, int eventHandlerNumber);

        void Decrement(int eventPosition);
    }
}
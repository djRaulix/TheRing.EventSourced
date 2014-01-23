namespace Thering.EventSourced.Eventing
{
    public interface IHandleEvent<in T> : IHandleEvent
    {
        void Handle(T @event);
    }
}
namespace Thering.EventSourced.Eventing.Handlers
{
    public interface IHandleEvent<in T> 
    {
        void Handle(T @event);
    }
}
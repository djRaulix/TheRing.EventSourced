namespace Thering.EventSourced.Eventing
{
    public interface IHandleEvent<in T> 
    {
        void Handle(T @event);
    }
}
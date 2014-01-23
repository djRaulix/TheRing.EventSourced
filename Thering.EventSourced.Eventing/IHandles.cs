namespace Thering.EventSourced.Eventing
{
    public interface IHandlesEvent<in T>
    {
        void Handles(T @event);
    }
}
namespace Thering.EventSourced.Eventing
{
    public interface IEventQueue
    {
        void Push(object @event);

        void Stop();
    }
}
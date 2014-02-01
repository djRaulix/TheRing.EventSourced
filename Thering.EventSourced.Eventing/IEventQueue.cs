namespace Thering.EventSourced.Eventing
{
    public interface IEventQueue
    {
        void Push(EventWithMetadata @event, int position);

        void Stop();
    }
}
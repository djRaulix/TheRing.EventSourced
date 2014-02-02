namespace Thering.EventSourced.Eventing.Handlers
{
    using Thering.EventSourced.Eventing.Events;

    public interface IEventQueue
    {
        void Push(EventWithMetadata @event, int position);

        void Stop();
    }
}
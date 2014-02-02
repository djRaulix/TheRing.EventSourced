namespace Thering.EventSourced.Eventing.Handlers
{
    using Thering.EventSourced.Eventing.Events;

    public interface IHandleEvent
    {
        void Handle(EventWithMetadata eventWithMetadata, int positon);
    }
}
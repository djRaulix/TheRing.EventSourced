namespace Thering.EventSourced.Eventing
{
    public interface IHandleEvent
    {
        void Handle(EventWithMetadata eventWithMetadata, int positon);
    }
}
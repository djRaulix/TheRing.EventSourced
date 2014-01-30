namespace Thering.EventSourced.Eventing
{
    public interface IEventHandler
    {
        void Handle(EventWithMetadata eventWithMetadata);
    }
}
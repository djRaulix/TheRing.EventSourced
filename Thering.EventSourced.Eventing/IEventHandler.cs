namespace Thering.EventSourced.Eventing
{
    public interface IEventHandler
    {
        void Handle(object @event);
    }
}
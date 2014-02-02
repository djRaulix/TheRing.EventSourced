namespace Thering.EventSourced.Eventing.Handlers
{
    using System;

    public interface IHandleError
    {
        void HandleError(object @event, Exception exception);
    }
}
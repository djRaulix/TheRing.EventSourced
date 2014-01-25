namespace Thering.EventSourced.Eventing
{
    using System;

    public interface IHandleError
    {
        void HandleError(object @event, Exception exception);
    }
}
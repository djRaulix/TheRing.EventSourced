namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using System.Collections.Generic;

    using EventStore.ClientAPI;

    #endregion

    public interface ITransformToEventData
    {
        #region Public Methods and Operators

        EventData Transform(object @event, IDictionary<string, object> headers);

        #endregion
    }
}
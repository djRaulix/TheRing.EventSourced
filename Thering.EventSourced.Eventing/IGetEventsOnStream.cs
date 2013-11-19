namespace Thering.EventSourced.Eventing
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IGetEventsOnStream
    {
        #region Public Methods and Operators

        IEnumerable<object> Get(string streamName, int? fromVersion = null, int? toVersion = null);

        IEnumerable<object> GetBackward(string streamName, int? fromVersion = null, int? toVersion = null);

        IEnumerable<EventWithMetadata> GetWithMetadata(string streamName, int? fromVersion = null, int? toVersion = null);

        IEnumerable<EventWithMetadata> GetBackwardWithMetadata(string streamName, int? fromVersion = null, int? toVersion = null);

        #endregion
    }
}
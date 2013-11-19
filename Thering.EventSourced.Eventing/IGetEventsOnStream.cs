namespace Thering.EventSourced.Eventing
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IGetEventsOnStream
    {
        #region Public Methods and Operators

        IEnumerable<object> Get(string streamName, int fromVersion, int toVersion, bool backWard = false);

        #endregion
    }
}
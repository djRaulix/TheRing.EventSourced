namespace TheRing.EventSourced.Core
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IGetEventsOnStream
    {
        #region Public Methods and Operators

        IEnumerable<object> Get(string streamName, int fromVersion = 0, int toVersion = int.MaxValue);

        #endregion
    }
}
namespace TheRing.EventSourced.GetEventStore.Exceptions
{
    public class StreamVersionException : StreamException
    {
        #region Constructors and Destructors

        public StreamVersionException(string streamName, int expectedVersion, int foundVersion)
            : base(
                streamName, 
                string.Concat(
                    "stream '{0}' was expected with ", 
                    string.Format("{0} version but was found with {1} version", expectedVersion, foundVersion)))
        {
        }

        #endregion
    }
}
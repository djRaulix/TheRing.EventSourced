namespace TheRing.EventSourced.GetEventStore.Exceptions
{
    public class StreamNotFoundException : StreamException
    {
        #region Constructors and Destructors

        public StreamNotFoundException(string streamName)
            : base(streamName, "stream '{0}' was not found.")
        {
            this.StreamName = streamName;
        }

        #endregion

        #region Public Properties

        public string StreamName { get; private set; }

        #endregion
    }

    public class StreamVersionException : StreamException
    {
        #region Constructors and Destructors

        public StreamVersionException(string streamName, int expectedVersion)
            : base(streamName, "stream '{0}' vwas not found.")
        {
            this.StreamName = streamName;
        }

        #endregion

        #region Public Properties

        public string StreamName { get; private set; }

        #endregion
    }
}
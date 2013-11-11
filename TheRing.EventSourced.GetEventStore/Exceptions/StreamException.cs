namespace TheRing.EventSourced.GetEventStore.Exceptions
{
    using System;

    public class StreamException : Exception
    {
        #region Constructors and Destructors

        public StreamException(string streamName, string message)
            : base(string.Format(message, streamName))
        {
            this.StreamName = streamName;
        }

        #endregion

        #region Public Properties

        public string StreamName { get; private set; }

        #endregion
    }
}
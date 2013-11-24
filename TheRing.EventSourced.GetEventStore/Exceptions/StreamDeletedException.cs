namespace TheRing.EventSourced.GetEventStore.Exceptions
{
    public class StreamDeletedException : StreamException
    {
        #region Constructors and Destructors

        public StreamDeletedException(string streamName)
            : base(streamName, "stream '{0}' was deleted.")
        {
        }

        #endregion

    }
}
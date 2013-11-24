namespace TheRing.EventSourced.GetEventStore.Exceptions
{
    public class StreamNotFoundException : StreamException
    {
        #region Constructors and Destructors

        public StreamNotFoundException(string streamName)
            : base(streamName, "stream '{0}' was not found.")
        {
        }

        #endregion
    }
}
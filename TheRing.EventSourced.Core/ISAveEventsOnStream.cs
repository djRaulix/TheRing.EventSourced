namespace TheRing.EventSourced.Core
{
    using System.Collections.Generic;

    public interface ISAveEventsOnStream
    {
        void Save(
            string streamName,
            IEnumerable<object> events,
            IDictionary<string, object> headers = null,
            int? expectedVersion = null);
    }
}
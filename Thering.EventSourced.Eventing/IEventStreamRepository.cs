namespace Thering.EventSourced.Eventing
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IEventStreamRepository
    {
        #region Public Methods and Operators

        IEnumerable<object> Get(string streamName, int fromVersion = StreamPosition.Start, int count = int.MaxValue);

        IEnumerable<object> GetBackward(
            string streamName, 
            int fromVersion = StreamPosition.End, 
            int count = int.MaxValue);

        IEnumerable<EventWithMetadata> GetBackwardWithMetadata(
            string streamName, 
            int fromVersion = StreamPosition.End, 
            int count = int.MaxValue);

        IEnumerable<EventWithMetadata> GetWithMetadata(
            string streamName, 
            int fromVersion = StreamPosition.Start, 
            int count = int.MaxValue);

        void Save(
            string streamName, 
            IEnumerable<object> events, 
            IDictionary<string, object> headers = null, 
            int? expectedVersion = null);

        void Save(
            string streamName, 
            object @event, 
            IDictionary<string, object> headers = null, 
            int? expectedVersion = null);

        #endregion
    }
}
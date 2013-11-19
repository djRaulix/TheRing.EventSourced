namespace TheRing.EventSourced.GetEventStore
{
    #region using

    using EventStore.ClientAPI;

    using Thering.EventSourced.Eventing;

    #endregion

    public interface IGetEventFromRecorded
    {
        #region Public Methods and Operators

        EventWithMetadata Get(RecordedEvent recordedEvent);

        #endregion
    }
}
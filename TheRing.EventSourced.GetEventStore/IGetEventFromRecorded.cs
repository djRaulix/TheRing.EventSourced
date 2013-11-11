namespace TheRing.EventSourced.GetEventStore
{
    using EventStore.ClientAPI;

    public interface IGetEventFromRecorded
    {
        #region Public Methods and Operators

        object Get(RecordedEvent recordedEvent);

        #endregion
    }
}
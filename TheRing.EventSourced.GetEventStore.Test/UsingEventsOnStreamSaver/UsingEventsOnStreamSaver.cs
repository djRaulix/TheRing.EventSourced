namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using TheRing.EventSourced.GetEventStore.Json;

    #endregion

    public abstract class UsingEventsOnStreamSaver : Specification
    {
        #region Properties

        protected EventsOnStreamSaver Saver { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.Saver = new EventsOnStreamSaver(this.Connection, new NewtonJsonSerializer());
        }

        #endregion
    }
}
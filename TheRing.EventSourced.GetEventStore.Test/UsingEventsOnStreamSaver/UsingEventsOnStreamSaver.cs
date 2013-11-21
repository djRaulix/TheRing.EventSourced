namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using TheRing.EventSourced.Core;

    #endregion

    public abstract class UsingEventsOnStreamSaver : Specification
    {
        #region Properties

        protected ISerializeEvent EventSerializer { get; set; }
        protected EventsOnStreamSaver Saver { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventSerializer = new EventSerializer(new NewtonJsonSerializer(), this.Aliaser);
            this.Saver = new EventsOnStreamSaver(this.Connection, this.EventSerializer);
        }

        #endregion
    }
}
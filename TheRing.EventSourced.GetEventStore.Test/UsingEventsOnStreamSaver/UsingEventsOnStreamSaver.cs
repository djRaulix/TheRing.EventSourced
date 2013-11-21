namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using TheRing.EventSourced.Core;

    #endregion

    public abstract class UsingEventsOnStreamSaver : Specification
    {
        #region Properties

        protected EventsOnStreamSaver Saver { get; set; }

        protected ISerializeEvent EventSerializer { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventSerializer = new EventSerializer(
                new NewtonJsonSerializer(), 
                new TypeAliaser(new ShortNameDefaultTypeAliasingStrategy()));
            this.Saver = new EventsOnStreamSaver(this.Connection, this.EventSerializer);
        }

        #endregion
    }
}
namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamGetter
{
    #region using

    using TheRing.EventSourced.Core;

    #endregion

    public abstract class UsingEventsOnStreamGetter : Specification
    {
        #region Properties

        protected ISerializeEvent EventSerializer { get; set; }
        protected EventsOnStreamGetter Getter { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventSerializer = new EventSerializer(
                new NewtonJsonSerializer(), 
                new TypeAliaser(new ShortNameDefaultTypeAliasingStrategy()));
            this.Getter = new EventsOnStreamGetter(this.Connection, this.EventSerializer);
        }

        #endregion
    }
}
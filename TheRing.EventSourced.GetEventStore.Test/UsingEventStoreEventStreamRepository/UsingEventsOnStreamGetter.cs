namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using TheRing.EventSourced.Core;

    #endregion

    public abstract class UsingEventStoreEventStreamRepository : Specification
    {
        #region Properties

        protected ISerializeEvent EventSerializer { get; set; }
        protected EventStoreEventStreamRepository EventStoreEventStreamRepository { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.EventSerializer = new EventSerializer(new ServiceStackJsonSerializer(), this.Aliaser);
            this.EventStoreEventStreamRepository = new EventStoreEventStreamRepository(
                this.Connection, 
                this.EventSerializer);
        }

        #endregion
    }
}
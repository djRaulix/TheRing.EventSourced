namespace TheRing.EventSourced.GetEventStore.Test
{
    #region using

    using System.Net;

    using EventStore.ClientAPI;

    using TheRing.Test;

    #endregion

    public abstract class Specification : SpecBase
    {
        #region Properties

        protected IEventStoreConnection Connection { get; set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.Connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            this.Connection.Connect();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            this.Connection.Close();
        }

        #endregion
    }
}
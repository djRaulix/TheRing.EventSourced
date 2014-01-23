namespace TheRing.EventSourced.GetEventStore.Test
{
    #region using

    using System.Net;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;
    using TheRing.Test;
    using TheRing.Test.Fakes;

    #endregion

    public abstract class Specification : SpecBase
    {
        #region Properties

        protected ITypeAliaser Aliaser { get; set; }
        protected IEventStoreConnection Connection { get; set; }

        #endregion

        #region Methods

        protected override void Cleanup()
        {
            base.Cleanup();
            this.Connection.Close();
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.Aliaser = new TypeAliaser().AddAlias(typeof(object).Name, typeof(object))
                .AddAlias(typeof(FakeEvent).Name, typeof(FakeEvent));
            this.Connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            this.Connection.Connect();
        }

        #endregion
    }
}
namespace TheRing.EventSourced.GetEventStore.Test
{
    #region using

    using System.Net;

    using EventStore.ClientAPI;

    using FakeItEasy;

    using TheRing.EventSourced.Core;

    using Thering.EventSourced.Eventing.Aliaser;

    using TheRing.EventSourced.GetEventStore.Serializers;
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
            var aliaserConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            aliaserConnection.Connect();
            this.Aliaser = new FakeTypeAlliaser(new EventStoreEventStreamRepository(aliaserConnection, new TypeSerializer(new ServiceStackJsonSerializer())))
                .AddAlias(typeof(object).Name, typeof(object))
                .AddAlias(typeof(FakeEvent).Name, typeof(FakeEvent));
            this.Connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            this.Connection.Connect();
        }

        #endregion
    }
}
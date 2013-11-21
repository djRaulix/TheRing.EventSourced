namespace TheRing.EventSourced.GetEventStore.Test
{
    #region using

    using System;
    using System.Net;

    using EventStore.ClientAPI;

    using TheRing.EventSourced.Core;
    using TheRing.Test;

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
                .AddAlias(typeof(Event).Name, typeof(Event));
            this.Connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            this.Connection.Connect();
        }

        #endregion

        protected class Event
        {
            #region Fields

            public readonly Guid No;

            #endregion

            #region Constructors and Destructors

            public Event(Guid no)
            {
                this.No = no;
            }

            #endregion
        }
    }
}
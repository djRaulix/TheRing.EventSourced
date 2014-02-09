namespace TheRing.EventSourced.GetEventStore.Test.UsingEventPublisher
{

    using System;
    using System.Collections.Generic;

    using FakeItEasy;

    using TheRing.EventSourced.Core;

    using Thering.EventSourced.Eventing;
    using Thering.EventSourced.Eventing.Handlers;

    public abstract class UsingEventPublisher : Specification
    {
        protected IEventQueue EventQueue { get; private set; }

        #region Properties

        protected GetEventStoreEventPublisher GetEventStoreEventBus { get; private set; }

        protected ISerializeEvent EventSerializer { get; private set; }

        protected EventStoreEventStreamRepository EventStoreEventStreamRepository { get; private set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.EventSerializer = this.InitEventSerializer();
            this.EventQueue = A.Fake<IEventQueue>();

            this.EventStoreEventStreamRepository = new EventStoreEventStreamRepository(this.Connection, this.EventSerializer);
            this.GetEventStoreEventBus = new GetEventStoreEventPublisher(this.Connection, this.EventSerializer, EventQueue);
        }

        protected virtual ISerializeEvent InitEventSerializer()
        {
            return new EventSerializer(new ServiceStackJsonSerializer(), this.Aliaser);
        }

        #endregion
    }
}
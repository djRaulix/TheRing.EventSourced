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
        #region Properties

        protected EventPublisher EventBus { get; private set; }

        protected virtual Func<Type, IEnumerable<IEventQueue>> EventQueuesFactory
        {
            get
            {
                return t => new[] { A.Fake<IEventQueue>() };
            }
        }

        protected ISerializeEvent EventSerializer { get; private set; }

        protected EventStoreEventStreamRepository EventStoreEventStreamRepository { get; private set; }

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.EventSerializer = this.InitEventSerializer();

            this.EventStoreEventStreamRepository = new EventStoreEventStreamRepository(this.Connection, this.EventSerializer);
            this.EventBus = new EventPublisher(this.Connection, this.EventSerializer, EventQueuesFactory);
        }

        protected virtual ISerializeEvent InitEventSerializer()
        {
            return new EventSerializer(new ServiceStackJsonSerializer(), this.Aliaser);
        }

        #endregion
    }
}
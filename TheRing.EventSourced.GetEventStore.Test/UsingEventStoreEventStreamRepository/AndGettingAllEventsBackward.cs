﻿namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    #endregion

    public class AndGettingAllEventsBackward : UsingEventStoreEventStreamRepository
    {
        #region Fields

        private readonly string StreamName = "AndGettingAllEventsBackward-" + Guid.NewGuid().ToString().Replace("-", "_");

        private readonly FakeEvent event1 = new FakeEvent(Guid.NewGuid());

        private readonly FakeEvent event2 = new FakeEvent(Guid.NewGuid());

        private IEnumerable<object> result;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAllEventsShouldBeReturned()
        {
            this.result.Count().Should().Be(2);
            this.result.Last().As<FakeEvent>().No.Should().Be(this.event1.No);
            this.result.First().As<FakeEvent>().No.Should().Be(this.event2.No);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.result = this.EventStoreEventStreamRepository.GetBackward(this.StreamName);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.Connection.AppendToStream(
                this.StreamName, 
                ExpectedVersion.EmptyStream, 
                new[] { this.EventSerializer.Serialize(this.event1), this.EventSerializer.Serialize(this.event2) });
        }

        #endregion
    }
}
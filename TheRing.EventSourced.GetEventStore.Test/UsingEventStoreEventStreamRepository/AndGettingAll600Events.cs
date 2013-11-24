namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndGettingAll600Events : UsingEventStoreEventStreamRepository
    {
        #region Fields

        private readonly string StreamName = "AndGettingAll600Events-" + Guid.NewGuid().ToString().Replace("-", string.Empty);

        private IEnumerable<object> result;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAll600EventsShouldBeReturned()
        {
            this.result.Count().Should().Be(600);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.result = this.EventStoreEventStreamRepository.Get(this.StreamName);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            var events = new List<EventData>();
            for (var i = 0; i < 600; i++)
            {
                events.Add(this.EventSerializer.Serialize(new object()));
            }

            this.Connection.AppendToStream(this.StreamName, ExpectedVersion.EmptyStream, events);
        }

        #endregion
    }
}
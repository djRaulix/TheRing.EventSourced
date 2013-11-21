namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamGetter
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndGettingSelectedEvents : UsingEventsOnStreamGetter
    {
        #region Fields

        private readonly string StreamName = "GetEventSoreTests-AndGettingSelectedEvents-" + Guid.NewGuid();

        private readonly Event event0 = new Event(Guid.NewGuid());

        private readonly Event event1 = new Event(Guid.NewGuid());

        private readonly Event event2 = new Event(Guid.NewGuid());

        private readonly Event event3 = new Event(Guid.NewGuid());

        private IEnumerable<object> result;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenSelectedEventsShouldBeReturned()
        {
            this.result.Count().Should().Be(2);
            this.result.First().As<Event>().No.Should().Be(this.event1.No);
            this.result.Last().As<Event>().No.Should().Be(this.event2.No);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.result = this.Getter.Get(this.StreamName, 1, 2);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.Connection.AppendToStream(
                this.StreamName, 
                ExpectedVersion.EmptyStream, 
                new[]
                    {
                        this.EventSerializer.Serialize(this.event0), this.EventSerializer.Serialize(this.event1), 
                        this.EventSerializer.Serialize(this.event2), this.EventSerializer.Serialize(this.event3)
                    });
        }

        #endregion
    }
}
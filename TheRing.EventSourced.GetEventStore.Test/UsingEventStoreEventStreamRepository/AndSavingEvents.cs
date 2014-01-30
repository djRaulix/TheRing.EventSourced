namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using System;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    #endregion

    public class AndSavingEvents : UsingEventStoreEventStreamRepository
    {
        #region Constants

        private const string StreamName = "AndSavingEvents";

        #endregion

        #region Fields

        private readonly FakeEvent event1 = new FakeEvent(Guid.NewGuid());

        private readonly FakeEvent event2 = new FakeEvent(Guid.NewGuid());

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenEventsShouldBeSaved()
        {
            var currentSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 2, false);
            this.EventSerializer.Deserialize(currentSlice.Events.First().OriginalEvent, currentSlice.Events.First().OriginalEventNumber)
                .Event.As<FakeEvent>()
                .No.Should()
                .Be(this.event2.No);
            this.EventSerializer.Deserialize(currentSlice.Events.Last().OriginalEvent, currentSlice.Events.Last().OriginalEventNumber)
                .Event.As<FakeEvent>()
                .No.Should()
                .Be(this.event1.No);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventStoreEventStreamRepository.Save(StreamName, new[] { this.event1, this.event2 });
        }

        #endregion
    }
}
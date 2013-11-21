namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using System;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndSavingEvents : UsingEventsOnStreamSaver
    {
        #region Constants

        private const string StreamName = "GetEventSoreTests-AndSavingEvents";

        #endregion

        #region Fields

        private readonly Event event1 = new Event(Guid.NewGuid());

        private readonly Event event2 = new Event(Guid.NewGuid());

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenEventsShouldBeSaved()
        {
            var currentSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 2, false);
            this.EventSerializer.Deserialize(currentSlice.Events.First().OriginalEvent)
                .Event.As<Event>()
                .No.Should()
                .Be(this.event2.No);
            this.EventSerializer.Deserialize(currentSlice.Events.Last().OriginalEvent)
                .Event.As<Event>()
                .No.Should()
                .Be(this.event1.No);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Saver.Save(StreamName, new[] { this.event1, this.event2 });
        }

        #endregion
    }
}
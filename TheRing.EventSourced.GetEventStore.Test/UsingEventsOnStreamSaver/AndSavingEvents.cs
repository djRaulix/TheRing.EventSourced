namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using System;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.EventSourced.Core;

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
            EventSerializer.Deserialize(currentSlice.Events.First().OriginalEvent).Event.As<Event>().No.Should().Be(this.event2.No);
            EventSerializer.Deserialize(currentSlice.Events.Last().OriginalEvent).Event.As<Event>().No.Should().Be(this.event1.No);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Saver.Save(StreamName, new[] { this.event1, this.event2 });
        }

        #endregion

        private class Event
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
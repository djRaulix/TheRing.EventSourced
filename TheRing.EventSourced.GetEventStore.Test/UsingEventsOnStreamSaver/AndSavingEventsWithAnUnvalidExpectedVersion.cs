namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using System;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndSavingEventsWithAnUnvalidExpectedVersion : UsingEventsOnStreamSaver
    {
        #region Constants

        private const string StreamName = "GetEventSoreTests-AndSavingEventsWithAnUnvalidExpectedVersion";

        #endregion

        #region Fields

        private AggregateException exception;

        private int expectedVersion;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenSavingEventsShouldThrowException()
        {
            this.exception.Should().NotBeNull();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            try
            {
                this.Saver.Save(StreamName, new[] { new object() }, null, this.expectedVersion);
            }
            catch (AggregateException ex)
            {
                this.exception = ex;
            }
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            var eventsSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 1, false);
            this.expectedVersion = eventsSlice.LastEventNumber == -1 ? 0 : eventsSlice.LastEventNumber - 1;
        }

        #endregion
    }
}
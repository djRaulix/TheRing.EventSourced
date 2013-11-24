namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using System;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndSavingEventsWithAValidExpectedVersion : UsingEventStoreEventStreamRepository
    {
        #region Constants

        private const string StreamName = "AndSavingEventsWithAValidExpectedVersion";

        #endregion

        #region Fields

        private Exception exception;

        private int expectedVersion;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenEventsShouldBeSaved()
        {
            this.exception.Should().BeNull();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            try
            {
                this.EventStoreEventStreamRepository.Save(
                    StreamName, 
                    new[] { new object() }, 
                    null, 
                    this.expectedVersion);
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            var eventsSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 1, false);
            this.expectedVersion = eventsSlice.LastEventNumber;
        }

        #endregion
    }
}
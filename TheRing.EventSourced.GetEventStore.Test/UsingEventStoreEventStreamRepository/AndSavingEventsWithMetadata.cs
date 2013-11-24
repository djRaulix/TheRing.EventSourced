namespace TheRing.EventSourced.GetEventStore.Test.UsingEventStoreEventStreamRepository
{
    #region using

    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndSavingEventsWithMetadata : UsingEventStoreEventStreamRepository
    {
        #region Constants

        private const string MetadataKey = "key";

        private const string MetadataValue = "value";

        private const string StreamName = "AndSavingEventsWithMetadata";

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenMetadataShouldBeSaved()
        {
            var currentSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 1, false);
            var metadata = this.EventSerializer.Deserialize(currentSlice.Events.First().OriginalEvent).Metadata;
            metadata[MetadataKey].Should().Be(MetadataValue);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventStoreEventStreamRepository.Save(
                StreamName, 
                new[] { new object() }, 
                new Dictionary<string, object> { { MetadataKey, MetadataValue } });
        }

        #endregion
    }
}
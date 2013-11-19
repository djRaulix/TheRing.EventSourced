namespace TheRing.EventSourced.GetEventStore.Test.UsingEventsOnStreamSaver
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EventStore.ClientAPI;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.EventSourced.GetEventStore.Json;

    #endregion

    public class AndSavingEventsWithMetadata : UsingEventsOnStreamSaver
    {
        #region Constants

        private const string MetadataKey = "key";

        private const string MetadataValue = "value";

        private const string StreamName = "GetEventSoreTests-AndSavingEventsWithMetadata";

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenMetadataShouldBeSaved()
        {
            var currentSlice = this.Connection.ReadStreamEventsBackward(StreamName, StreamPosition.End, 1, false);
            var deserializer = new NewtonJsonSerializer();
            var metadata = deserializer.Get(currentSlice.Events.First().OriginalEvent).Metadata;
            metadata[MetadataKey].Should().Be(MetadataValue);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Saver.Save(StreamName, new[] { new object() }, new Dictionary<string, object> { { MetadataKey, MetadataValue } });
        }

        #endregion
    }
}


namespace TheRing.EventSourced.GetEventStore.Test.UsingEventPublisher
{
    using Thering.EventSourced.Eventing;

    using TheRing.Test.Fakes;

    using System;
    using System.Collections.Generic;
    using System.Threading;

    using EventStore.ClientAPI;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    public class AndSavingOnSystemStream : UsingEventPublisher
    {
        private ISerializeEvent fakeEventSerializer;

        private const string StreamName = "$TestSystemStream";

        private readonly IEventQueue eventQueue = A.Fake<IEventQueue>();

        protected override Func<Type, IEnumerable<IEventQueue>> EventQueuesFactory
        {
            get
            {
                return t => new[] { eventQueue };
            }
        }

        protected override void BecauseOf()
        {
            base.BecauseOf();
            EventStoreEventStreamRepository.Save(StreamName, new FakeEvent(Guid.NewGuid()));
        }

        protected override ISerializeEvent InitEventSerializer()
        {
            fakeEventSerializer = A.Fake<ISerializeEvent>();
            fakeEventSerializer.CallsTo(s => s.Serialize(A<object>.Ignored, A<IDictionary<string, object>>.Ignored)).Returns(new EventData(Guid.NewGuid(),"FakeEvent", true, new byte[] { 1 }, new byte[] { 1}));
            return fakeEventSerializer;
        }

        [Test]
        public void ThenEventShouldNotBeDeserialize()
        {
            //Attente car operation asynchrone
            Thread.Sleep(1000);
            fakeEventSerializer.CallsTo(s => s.Deserialize(A<RecordedEvent>.Ignored)).MustHaveHappened(Repeated.Never);
        }

        [Test]
        public void ThenEventShouldNotBeSendToQueue()
        {
            //Attente car operation asynchrone
            Thread.Sleep(1000);
            eventQueue.CallsTo(q => q.Push(A<object>.Ignored)).MustHaveHappened(Repeated.Never);
        }
    }
}
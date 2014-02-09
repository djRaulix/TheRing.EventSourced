namespace TheRing.EventSourced.GetEventStore.Test.UsingEventPublisher
{
    using System;
    using System.Threading;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using Thering.EventSourced.Eventing.Events;

    using TheRing.Test.Fakes;

    public class AndSavingAggregateEvent : UsingEventPublisher
    {
       
        private FakeEvent fakeEvent;
        private const string StreamName = "AndSavingEventWithTwoSubscribers";

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.fakeEvent = new FakeEvent(Guid.NewGuid());
            EventStoreEventStreamRepository.Save(StreamName, this.fakeEvent);
        }

        [Test]
        public void ThenEventShouldBeSendToEventQueue()
        {
            Thread.Sleep(1000);
            EventQueue.CallsTo(queue => queue.Push(A<EventWithMetadata>.That.Matches(e => fakeEvent.No.Equals(((FakeEvent)e.Event).No)), A<int>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
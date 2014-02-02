namespace TheRing.EventSourced.GetEventStore.Test.UsingEventPublisher
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using Thering.EventSourced.Eventing;
    using Thering.EventSourced.Eventing.Events;
    using Thering.EventSourced.Eventing.Handlers;

    using TheRing.Test.Fakes;

    public class AndSavingEventWithTwoSubscribers : UsingEventPublisher
    {
        private readonly ICollection<IEventQueue> eventQueues = new[]
                                                                    {
                                                                        A.Fake<IEventQueue>(),
                                                                        A.Fake<IEventQueue>()
                                                                    };

        private FakeEvent fakeEvent;

        protected override Func<Type, IEnumerable<IEventQueue>> EventQueuesFactory
        {
            get
            {
                return t => eventQueues;
            }
        }

        private const string StreamName = "AndSavingEventWithTwoSubscribers";

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.fakeEvent = new FakeEvent(Guid.NewGuid());
            EventStoreEventStreamRepository.Save(StreamName, this.fakeEvent);
        }

        [Test]
        public void ThenAllSubscribersShouldReceiveTheRightEvent()
        {
            //Attente car operation asynchrone
            Thread.Sleep(1000);
            foreach (var eventQueue in eventQueues)
            {
                eventQueue.CallsTo(queue => queue.Push(A<EventWithMetadata>.That.Matches(e => fakeEvent.No.Equals(((FakeEvent)e.Event).No)), A<int>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}
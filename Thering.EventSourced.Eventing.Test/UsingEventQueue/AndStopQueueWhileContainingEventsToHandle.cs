namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using System.Linq;
    using System.Threading;

    using FluentAssertions;

    using NUnit.Framework;

    using Thering.EventSourced.Eventing.Events;

    using TheRing.Test.Fakes;

    public class AndStopQueueWhileContainingEventsToHandle : UsingEventQueue
    {
        private const int EventBeforeStopCount = 7;

        private const int EventAfterStopCount = 5;

        protected override void Cleanup()
        {
            //Queue should allready stopped
        }

        protected override void BecauseOf()
        {
            base.BecauseOf();

            for (var i = 1; i <= EventBeforeStopCount; i++)
            {
                this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(i)), 0);
            }
            this.EventQueue.Stop();

            for (var i = EventBeforeStopCount; i <= EventAfterStopCount + EventBeforeStopCount; i++)
            {
                this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(i)), 0);
            }
        }

        [Test]
        public void ThenAllEventAddedBeforeStopShouldHaveBeenHandle()
        {
            Thread.Sleep(1500);
            this.FakeEventHandler.HandledIntFakeEvent.Count(h => h.Key <= EventBeforeStopCount).Should().Be(EventBeforeStopCount);
        }

        [Test]
        public void ThenAllEventAddedAfterStopShouldNotHaveBeenHandle()
        {
            Thread.Sleep(1500);
            this.FakeEventHandler.HandledIntFakeEvent.Any(h => h.Key > EventBeforeStopCount).Should().Be(false);
        }
    }
}
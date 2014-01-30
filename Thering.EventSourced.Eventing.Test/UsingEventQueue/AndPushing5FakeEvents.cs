namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using System.Threading;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    public class AndPushing5FakeEvents : UsingEventQueue
    {
        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(1),0));
            this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(2),0));
            this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(3),0));
            this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(4),0));
            this.EventQueue.Push(new EventWithMetadata(new IntFakeEvent(5),0));
        }

        [Test]
        public void ThenAllEventShouldHaveBeenHandledInRightOrder()
        {
            Thread.Sleep(1200);
            FakeEventHandler.HandledIntFakeEvent.Count.Should().Be(5);
            foreach (var intFakeEvent in this.FakeEventHandler.HandledIntFakeEvent)
            {
                intFakeEvent.Key.Should().Be(intFakeEvent.Value.No);
            }
        }
    }
}
namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using System;
    using System.Threading;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    public class AndPushingaFakeEvent : UsingEventQueue
    {
        private readonly Guid eventId = Guid.NewGuid();

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventQueue.Push(new FakeEvent(this.eventId));
        }

        [Test]
        public void ThenEventShouldBeHandle()
        {
            Thread.Sleep(1000);
            this.FakeEventHandler.LastHandledEvent.No.Should().Be(this.eventId);
        }
    }
}
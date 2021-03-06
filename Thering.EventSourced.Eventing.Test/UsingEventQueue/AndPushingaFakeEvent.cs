﻿namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using System;
    using System.Threading;

    using FluentAssertions;

    using NUnit.Framework;

    using Thering.EventSourced.Eventing.Events;

    using TheRing.Test.Fakes;

    public class AndPushingaFakeEvent : UsingEventQueue
    {
        private readonly Guid eventId = Guid.NewGuid();

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventQueue.Push(new EventWithMetadata(new FakeEvent(this.eventId)), 0);
        }

        [Test]
        public void ThenEventShouldBeHandle()
        {
            Thread.Sleep(1000);
            this.FakeEventHandler.LastHandledEvent.No.Should().Be(this.eventId);
        }
    }
}
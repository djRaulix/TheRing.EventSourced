namespace Thering.EventSourced.Eventing.Test.UsingEventHandler
{
    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    public class AndHandlingAnEvent : UsingEventHandler
    {

        private const int EventPosition = 54;
        private readonly Guid eventId = Guid.NewGuid();

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventHandler.Handle(new EventWithMetadata(new FakeEvent(this.eventId)), EventPosition);   
        }

        [Test]
        public void ThenEventPositionShouldBeSave()
        {
            this.EventPositionRepository.CallsTo(repo => repo.Save(A<Type>.That.Matches(t => t == typeof(FakeEventHandler)), A<int>.That.Matches(p => p == EventPosition)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ThenEventShouldBeHandle()
        {
            this.FakeEventHandler.LastHandledEvent.No.Should().Be(this.eventId);
        }
    }
}
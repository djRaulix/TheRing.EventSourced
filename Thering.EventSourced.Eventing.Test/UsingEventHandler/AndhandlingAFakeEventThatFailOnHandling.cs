namespace Thering.EventSourced.Eventing.Test.UsingEventHandler
{
    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using Thering.EventSourced.Eventing.Events;

    using TheRing.Test.Fakes;

    public class AndhandlingAFakeEventThatFailOnHandling : UsingEventHandler
    {
        private readonly Guid eventId = Guid.NewGuid();

        private readonly FakeEventHandlerThatFail fakeEventHandlerThatFail = new FakeEventHandlerThatFail();

        protected override FakeEventHandler FakeEventHandler
        {
            get
            {
                return fakeEventHandlerThatFail;
            }
        }

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventHandler.Handle(new EventWithMetadata(new FakeEvent(this.eventId)), 0);
        }

        [Test]
        public void ThenEventShouldBeHandleByErrorHandler()
        {
            var thrownErrorMessage = fakeEventHandlerThatFail.ThrownException.Message;

            this.ErrorHandler.CallsTo(handler => 
                handler.HandleError(A<FakeEvent>.That.Matches(e => e.No == this.eventId), A<Exception>.That.Matches(e => e.Message.Equals(thrownErrorMessage))))
                             .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ThenEventPositionShouldNotBeSave()
        {
            this.EventPositionRepository.CallsTo(repo => repo.Save(A<Type>.Ignored, A<int>.Ignored))
                .MustHaveHappened(Repeated.Never);
        }
    }
}
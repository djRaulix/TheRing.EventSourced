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
        private int eventPosition;

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
            this.eventPosition = 10;
            this.EventHandler.Handle(new EventWithMetadata(new FakeEvent(this.eventId)), this.eventPosition);
        }

        [Test]
        public void ThenEventShouldBeHandleByErrorHandler()
        {
            var thrownErrorMessage = fakeEventHandlerThatFail.ThrownException.Message;

            this.ErrorHandler.CallsTo(handler =>
                handler.HandleError(
                        A<FakeEvent>.That.Matches(e => e.No == this.eventId), 
                        A<int>.That.Matches(pos => pos == eventPosition), 
                        A<Type>.That.Matches(t => typeof(FakeEventHandlerThatFail) == t), 
                        A<Exception>.That.Matches(e => e.Message.Equals(thrownErrorMessage))))
                             .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ThenEventPositionTokenShouldBeDecrement()
        {
            this.EventPositionManager.CallsTo(repo => repo.Decrement(A<int>.That.Matches(p => p == eventPosition)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
namespace Thering.EventSourced.Eventing.Test.UsingEventHandler
{
    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    public class AndhandlingAFakeEventThatFailOnHandling : UsingEventHandler
    {
        private readonly Guid eventId = Guid.NewGuid();
       

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventHandler.Handle(new FakeEvent(this.eventId));
        }

        [Test]
        public void ThenEventShouldBeHandleByErrorHandler()
        {
            var thrownErrorMessage = ((FakeEventHandlerThatFail)this.FakeEventHandler).ThrownException.Message;

            this.ErrorHandler.CallsTo(handler => 
                handler.HandleError(A<FakeEvent>.That.Matches(e => e.No == this.eventId), A<Exception>.That.Matches(e => e.Message.Equals(thrownErrorMessage))))
                             .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
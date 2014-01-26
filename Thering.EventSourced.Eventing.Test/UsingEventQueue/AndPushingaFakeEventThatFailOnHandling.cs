namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using System;
    using System.Threading;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using TheRing.Test.Fakes;

    public class AndPushingaFakeEventThatFailOnHandling : UsingEventQueue
    {
        private readonly Guid eventId = Guid.NewGuid();
        private readonly FakeEventHandlerThatFail fakeEventHandler = new FakeEventHandlerThatFail();

        protected override FakeEventHandler FakeEventHandler
        {
            get
            {
                return fakeEventHandler;
            }
        }

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventQueue.Push(new FakeEvent(this.eventId));
        }

        [Test]
        public void ThenEventShouldBeHandleByErrorHandler()
        {
            Thread.Sleep(1000);
            var thrownErrorMessage = ((FakeEventHandlerThatFail)FakeEventHandler).ThrownException.Message;

            this.ErrorHanlder.CallsTo(handler => 
                handler.HandleError(A<FakeEvent>.That.Matches(e => e.No == this.eventId), A<Exception>.That.Matches(e => e.Message.Equals(thrownErrorMessage))))
                             .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
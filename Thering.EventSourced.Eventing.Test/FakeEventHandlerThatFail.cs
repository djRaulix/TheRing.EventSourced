namespace Thering.EventSourced.Eventing.Test
{
    using System;

    using TheRing.Test.Fakes;

    public class FakeEventHandlerThatFail : FakeEventHandler
    {
        public Exception ThrownException { get; private set; }

        #region Overrides of FakeEventHandler

        public override void Handle(FakeEvent @event)
        {
            this.ThrownException = new Exception("Test exception");
            throw this.ThrownException;
        }

        #endregion
    }
}
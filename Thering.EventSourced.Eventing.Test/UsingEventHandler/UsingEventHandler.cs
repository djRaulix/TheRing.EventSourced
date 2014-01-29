namespace Thering.EventSourced.Eventing.Test.UsingEventHandler
{
    using FakeItEasy;

    using TheRing.Test;

    public abstract class UsingEventHandler : SpecBase
    {
        private readonly FakeEventHandlerThatFail fakeEventHandler = new FakeEventHandlerThatFail();

        protected FakeEventHandler FakeEventHandler
        {
            get
            {
                return this.fakeEventHandler;
            }
        }

        protected IHandleError ErrorHandler { get; private set; }

        protected EventHandler EventHandler { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.ErrorHandler = A.Fake<IHandleError>();
            this.EventHandler = new EventHandler(this.FakeEventHandler, this.ErrorHandler);
        }
    }
}
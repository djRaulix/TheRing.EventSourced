namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using FakeItEasy;

    using TheRing.Test;

    public abstract class UsingEventQueue : SpecBase
    {
        private readonly FakeEventHandler fakeEventHandler = new FakeEventHandler();

        protected virtual FakeEventHandler FakeEventHandler
        {
            get
            {
                return fakeEventHandler;
            }
        }

        protected IHandleError ErrorHanlder { get; private set; }

        protected EventQueue EventQueue { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            ErrorHanlder = A.Fake<IHandleError>();
            this.EventQueue = new EventQueue(this.FakeEventHandler, ErrorHanlder);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            this.EventQueue.Stop();
        }
    }
}

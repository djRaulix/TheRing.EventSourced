namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using TheRing.Test;

    public abstract class UsingEventQueue : SpecBase
    {
        protected FakeEventHandler FakeEventHandler { get; private set; }

        protected EventQueue<FakeEventHandler> EventQueue { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.FakeEventHandler = new FakeEventHandler();
            this.EventQueue = new EventQueue<FakeEventHandler>(this.FakeEventHandler);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            this.EventQueue.Stop();
        }
    }
}

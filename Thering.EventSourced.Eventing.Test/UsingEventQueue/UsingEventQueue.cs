namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using TheRing.Test;

    public abstract class UsingEventQueue : SpecBase
    {
        protected FakeEventHandler FakeEventHandler { get; private set; }

        protected EventQueue EventQueue { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.FakeEventHandler = new FakeEventHandler();
            this.EventQueue = new EventQueue(this.FakeEventHandler);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            this.EventQueue.Stop();
        }
    }
}

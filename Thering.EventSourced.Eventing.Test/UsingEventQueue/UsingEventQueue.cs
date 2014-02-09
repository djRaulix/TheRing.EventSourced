namespace Thering.EventSourced.Eventing.Test.UsingEventQueue
{
    using FakeItEasy;

    using Thering.EventSourced.Eventing.Handlers;
    using Thering.EventSourced.Eventing.Repositories;

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

        protected EventHandlerQueue EventQueue { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventQueue = new EventHandlerQueue(new EventHandler(this.FakeEventHandler, A.Fake<IHandleError>(), A.Fake<IEventPositionRepository>()));
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            this.EventQueue.Stop();
        }
    }
}

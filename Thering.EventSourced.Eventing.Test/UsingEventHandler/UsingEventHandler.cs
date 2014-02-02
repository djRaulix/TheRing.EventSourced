namespace Thering.EventSourced.Eventing.Test.UsingEventHandler
{
    using FakeItEasy;

    using Thering.EventSourced.Eventing.Handlers;
    using Thering.EventSourced.Eventing.Repositories;

    using TheRing.Test;

    using EventHandler = Thering.EventSourced.Eventing.Handlers.EventHandler;

    public abstract class UsingEventHandler : SpecBase
    {
        private readonly FakeEventHandler fakeEventHandler = new FakeEventHandler();

        protected virtual FakeEventHandler FakeEventHandler
        {
            get
            {
                return this.fakeEventHandler;
            }
        }
        protected IHandleError ErrorHandler { get; private set; }

        protected EventHandler EventHandler { get; private set; }

        protected IEventPositionRepository EventPositionRepository { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.ErrorHandler = A.Fake<IHandleError>();
            this.EventPositionRepository = A.Fake<IEventPositionRepository>();
            this.EventHandler = new EventHandler(FakeEventHandler, this.ErrorHandler, EventPositionRepository);
        }
    }
}
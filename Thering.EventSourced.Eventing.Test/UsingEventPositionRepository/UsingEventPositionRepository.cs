namespace Thering.EventSourced.Eventing.Test.UsingEventPositionRepository
{
    using FakeItEasy;

    using Thering.EventSourced.Eventing.Repositories;

    using TheRing.Test;

    public abstract class UsingEventPositionRepository : SpecBase
    {
        protected IEventStreamRepository EventStreamRepository { get; private set; }

        protected EventPositionRepository EventPositionRepository { get; private set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventStreamRepository = A.Fake<IEventStreamRepository>();
            EventPositionRepository = new EventPositionRepository(this.EventStreamRepository);
        }
    }
}
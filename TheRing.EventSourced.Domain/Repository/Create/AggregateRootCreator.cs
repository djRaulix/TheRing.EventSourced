namespace TheRing.EventSourced.Domain.Repository.Create
{
    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    public class AggregateRootCreator : ICreateAggregateRoot
    {
        private readonly IAggregateRootFactory factory;

        public AggregateRootCreator(IAggregateRootFactory factory)
        {
            this.factory = factory;
        }

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var agg = this.factory.New<TAgg>();
            agg.Id = id;
            return agg;
        }
    }
}
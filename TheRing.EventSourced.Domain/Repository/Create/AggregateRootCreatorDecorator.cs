namespace TheRing.EventSourced.Domain.Repository.Create
{
    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    public abstract class AggregateRootCreatorDecorator : ICreateAggregateRoot
    {
        private readonly ICreateAggregateRoot decorated;

        protected AggregateRootCreatorDecorator(ICreateAggregateRoot decorated)
        {
            this.decorated = decorated;
        }

        #region Implementation of ICreateEventSourcedStrategy

        public virtual TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            return this.decorated.Create<TAgg>(id);
        }

        #endregion
    }
}
namespace TheRing.EventSourced.Domain.Repository.Get
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public abstract class AggregateRootGetterDecorator : IGetAggregateRoot
    {
        #region Fields

        private readonly IGetAggregateRoot decorated;

        #endregion

        #region Constructors and Destructors

        protected AggregateRootGetterDecorator(IGetAggregateRoot decorated)
        {
            this.decorated = decorated;
        }

        #endregion

        #region Public Methods and Operators

        public virtual TAgg Get<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            return this.decorated.Get<TAgg>(id);
        }

        #endregion
    }
}
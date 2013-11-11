namespace TheRing.EventSourced.Domain.Repository.Save
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public abstract class AggregateRootSaverDecorator : ISaveAggregateRoot
    {
        #region Fields

        private readonly ISaveAggregateRoot decorated;

        #endregion

        #region Constructors and Destructors

        protected AggregateRootSaverDecorator(ISaveAggregateRoot decorated)
        {
            this.decorated = decorated;
        }

        #endregion

        #region Public Methods and Operators

        public virtual void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot
        {
            this.decorated.Save(aggregateRoot);
        }

        #endregion
    }
}
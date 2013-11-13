namespace TheRing.EventSourced.Domain.Repository.Create
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class DiAggregateRootCreator : ICreateAggregateRoot
    {
        #region Fields

        private readonly IServiceProvider resolver;

        #endregion

        #region Constructors and Destructors

        public DiAggregateRootCreator(IServiceProvider resolver)
        {
            this.resolver = resolver;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var agg = (TAgg)this.resolver.GetService(typeof(TAgg));
            agg.Id = id;
            return agg;
        }

        #endregion
    }
}
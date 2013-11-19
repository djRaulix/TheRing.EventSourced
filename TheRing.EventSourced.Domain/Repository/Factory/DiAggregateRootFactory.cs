namespace TheRing.EventSourced.Domain.Repository.Factory
{
    #region using

    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class DiAggregateRootFactory : IAggregateRootFactory
    {
        #region Fields

        private readonly IServiceProvider resolver;

        #endregion

        #region Constructors and Destructors

        public DiAggregateRootFactory(IServiceProvider resolver)
        {
            this.resolver = resolver;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create<TAgg>() where TAgg : AggregateRoot
        {
            return (TAgg)this.resolver.GetService(typeof(TAgg));
        }

        #endregion
    }
}
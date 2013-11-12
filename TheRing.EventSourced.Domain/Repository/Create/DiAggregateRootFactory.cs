namespace TheRing.EventSourced.Domain.Repository.Create
{
    using System;

    using TheRing.EventSourced.Domain.Aggregate;

    public class DiAggregateRootFactory : IAggregateRootFactory
    {
        #region Fields

        private readonly Func<Type, object> resolver;

        #endregion

        #region Constructors and Destructors

        public DiAggregateRootFactory(Func<Type, object> resolver)
        {
            this.resolver = resolver;
        }

        #endregion

        #region Public Methods and Operators

        public T New<T>() where T : AggregateRoot
        {
            return (T)this.resolver(typeof(T));
        }

        #endregion
    }
}
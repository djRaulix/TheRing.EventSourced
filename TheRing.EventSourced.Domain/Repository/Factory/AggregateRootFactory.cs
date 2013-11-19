namespace TheRing.EventSourced.Domain.Repository.Factory
{
    #region using

    using TheRing.EventSourced.Core.Reflection.Activator;
    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class AggregateRootFactory : IAggregateRootFactory
    {
        #region Public Methods and Operators

        public TAgg Create<TAgg>() where TAgg : AggregateRoot
        {
            return Activator<TAgg>.CreateInstance();
        }

        #endregion
    }
}
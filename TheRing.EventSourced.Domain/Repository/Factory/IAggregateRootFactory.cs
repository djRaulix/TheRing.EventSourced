namespace TheRing.EventSourced.Domain.Repository.Factory
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IAggregateRootFactory
    {
        #region Public Methods and Operators

        TAgg Create<TAgg>() where TAgg : AggregateRoot;

        #endregion
    }
}
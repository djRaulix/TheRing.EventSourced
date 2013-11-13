namespace TheRing.EventSourced.Application
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public interface IRunCommand<in TAgg, in TCommand>
        where TAgg : AggregateRoot
    {
        #region Public Methods and Operators

        void Run(TAgg agg, TCommand command);

        #endregion
    }
}
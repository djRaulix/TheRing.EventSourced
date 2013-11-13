namespace TheRing.EventSourced.Application
{
    public interface IRunCommand<in TCommand>
    {
        #region Public Methods and Operators

        void Run(TCommand command);

        #endregion
    }
}
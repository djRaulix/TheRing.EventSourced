namespace TheRing.EventSourced.Application
{
    public class CommandHandler<TCommand> : Handler<TCommand>
    {
        #region Fields

        private readonly IRunCommand<TCommand> runner;

        #endregion

        #region Constructors and Destructors

        public CommandHandler(IRunCommand<TCommand> runner)
        {
            this.runner = runner;
        }

        #endregion

        #region Methods

        protected override void HandleCommand(TCommand command)
        {
            this.runner.Run(command);
        }

        #endregion
    }
}
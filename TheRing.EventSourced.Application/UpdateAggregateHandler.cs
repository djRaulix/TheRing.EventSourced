namespace TheRing.EventSourced.Application
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository;

    #endregion

    public class UpdateAggregateHandler<TAgg, TCommand> : Handler<TCommand>
        where TAgg : AggregateRoot where TCommand : UpdateCommand
    {
        #region Fields

        private readonly IAggregateRootRepository repository;

        private readonly IRunCommand<TAgg, TCommand> runCommand;

        #endregion

        #region Constructors and Destructors

        public UpdateAggregateHandler(IAggregateRootRepository repository, IRunCommand<TAgg, TCommand> runCommand)
        {
            this.repository = repository;
            this.runCommand = runCommand;
        }

        #endregion

        #region Methods

        protected override void HandleCommand(TCommand command)
        {
            var agg = this.repository.Get<TAgg>(command.AggregateRootId);
            this.runCommand.Run(agg, command);
            this.repository.Save(agg);
        }

        #endregion
    }
}
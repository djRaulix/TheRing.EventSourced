namespace TheRing.EventSourced.Application
{
    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.Domain.Aggregate;
    using TheRing.EventSourced.Domain.Repository;

    public class CreateAggregateHandler<TAgg, TCommand> : Handler<TCommand>
        where TAgg : AggregateRoot
        where TCommand : CreateCommand
    {
        #region Fields

        private readonly IAggregateRootRepository repository;

        private readonly IRunCommand<TAgg, TCommand> runCommand;

        #endregion

        #region Constructors and Destructors

        public CreateAggregateHandler(
            IAggregateRootRepository repository, 
            IRunCommand<TAgg, TCommand> runCommand)
        {
            this.repository = repository;
            this.runCommand = runCommand;
        }

        #endregion

        #region Methods

        protected override void HandleCommand(TCommand command)
        {
            var agg = this.repository.Create<TAgg>(command.AggregateRootId);
            this.runCommand.Run(agg, command);
            this.repository.Save(agg);
        }

        #endregion
    }
}
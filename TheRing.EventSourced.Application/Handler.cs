namespace TheRing.EventSourced.Application
{
    #region using

    using System;

    #endregion

    public abstract class Handler<TCommand> : IHandle<TCommand>
    {
        #region Public Methods and Operators

        public Result Handle(TCommand command)
        {
            try
            {
                this.HandleCommand(command);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.GetType().Name, ex.Message);
            }

            return new Result(true);
        }

        #endregion

        #region Methods

        protected abstract void HandleCommand(TCommand command);

        #endregion
    }
}
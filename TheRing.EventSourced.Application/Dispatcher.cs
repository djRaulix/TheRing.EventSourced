namespace TheRing.EventSourced.Application
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public class Dispatcher : IDispatch
    {
        #region Fields

        private readonly IServiceProvider resolver;

        #endregion

        #region Constructors and Destructors

        public Dispatcher(IServiceProvider resolver)
        {
            this.resolver = resolver;
        }

        #endregion

        #region Public Methods and Operators

        public Result Dispatch<T>(T command)
        {
            return ((IHandle<T>)this.resolver.GetService(typeof(IHandle<T>))).Handle(command);
        }

        public Task<Result> DispatchAsync<T>(T command)
        {
            return new Task<Result>(() => this.Dispatch(command));
        }

        #endregion
    }
}
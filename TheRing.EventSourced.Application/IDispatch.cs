namespace TheRing.EventSourced.Application
{
    #region using

    using System.Threading.Tasks;

    #endregion

    public interface IDispatch
    {
        #region Public Methods and Operators

        Result Dispatch<T>(T command);

        Task<Result> DispatchAsync<T>(T command);

        #endregion
    }
}
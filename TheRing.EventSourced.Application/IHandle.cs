namespace TheRing.EventSourced.Application
{
    public interface IHandle<in T>
    {
        #region Public Methods and Operators

        Result Handle(T command);

        #endregion
    }
}
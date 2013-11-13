namespace TheRing.EventSourced.Application
{
    public class Result
    {
        #region Fields

        public readonly string ErrorMessage;

        public readonly string ErrorName;

        public readonly bool Ok;

        #endregion

        #region Constructors and Destructors

        public Result(bool ok)
        {
            this.Ok = ok;
        }

        public Result(bool ok, string errorName, string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorName = errorName;
        }

        #endregion
    }
}
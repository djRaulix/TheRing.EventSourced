namespace TheRing.EventSourced.Application
{
    public class Result
    {
        public readonly string ErrorMessage;

        public readonly string ErrorName;

        public readonly bool Ok;

        public Result(bool ok)
        {
            this.Ok = ok;
        }

        public Result(bool ok, string errorName, string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorName = errorName;
        }
    }
}
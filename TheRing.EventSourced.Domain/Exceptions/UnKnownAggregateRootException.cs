namespace TheRing.EventSourced.Domain.Exceptions
{
    #region using

    using System;

    #endregion

    public class UnKnownAggregateRootException : Exception
    {
        #region Constructors and Destructors

        public UnKnownAggregateRootException(Guid id)
            : base(string.Format("Unknown Event sourced id ({0})", id))
        {
        }

        #endregion
    }
}
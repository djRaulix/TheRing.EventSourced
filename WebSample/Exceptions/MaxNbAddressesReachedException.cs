namespace WebSample.Exceptions
{
    #region using

    using System;

    #endregion

    public class MaxNbAddressesReachedException : Exception
    {
        #region Constructors and Destructors

        public MaxNbAddressesReachedException()
            : base("can't add another address")
        {
        }

        #endregion
    }
}
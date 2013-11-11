namespace WebSample.Domain.User
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class UserState : AggregateRootState
    {
        #region Properties

        internal int NbAddresses { get; set; }

        #endregion

        #region Public Methods and Operators

        public void When(UserAddressAdded @event)
        {
            this.NbAddresses++;
        }

        #endregion
    }
}
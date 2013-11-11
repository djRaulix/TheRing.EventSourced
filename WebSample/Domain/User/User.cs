namespace WebSample.Domain.User
{
    #region using

    using TheRing.EventSourced.Domain.Aggregate;

    using WebSample.Exceptions;

    #endregion

    public class User : AggregateRoot<UserState>
    {
        #region Constants

        private const int MaxAddresses = 5;

        #endregion

        #region Public Methods and Operators

        public void AddAddress(string address)
        {
            if (this.State.NbAddresses >= MaxAddresses)
            {
                throw new MaxNbAddressesReachedException();
            }

            this.Apply(new UserAddressAdded(address, this.State.NbAddresses < MaxAddresses - 1));
        }

        public void Confirm()
        {
            this.Apply(new UserConfirmed());
        }

        public void Create(string firstName, string lastName)
        {
            this.Apply(new UserCreated(firstName, lastName, true));
        }

        public void Delete()
        {
            this.Apply(new UserDeleted());
        }

        #endregion
    }
}
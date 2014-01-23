namespace WebSample.Domain.User.Events
{
    public class UserAddressAdded
    {
        #region Fields

        public readonly string Address;

        public readonly bool CanAddAddress;

        #endregion

        #region Constructors and Destructors

        public UserAddressAdded(string address, bool canAddAddress)
        {
            this.Address = address;
            this.CanAddAddress = canAddAddress;
        }

        #endregion
    }
}
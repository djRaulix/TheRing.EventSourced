namespace WebSample.Domain.User
{
    public class UserCreated
    {
        #region Fields

        public readonly bool CanAddAddress;

        public readonly string FirstName;

        public readonly string LastName;

        #endregion

        #region Constructors and Destructors

        public UserCreated(string firstName, string lastName, bool canAddAddress)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CanAddAddress = canAddAddress;
        }

        #endregion
    }
}
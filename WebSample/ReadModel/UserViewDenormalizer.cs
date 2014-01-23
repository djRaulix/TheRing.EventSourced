namespace WebSample.ReadModel
{
    using System.Collections.ObjectModel;

    using Thering.EventSourced.Eventing;
    using WebSample.Domain.User;

    public class UserViewDenormalizer : IHandlesEvent<UserAddressAdded>,
                                        IHandlesEvent<UserCreated>
    {
        public void Handles(UserAddressAdded @event)
        {
            Database.Adresses.Add(@event.Address);
        }

        public void Handles(UserCreated @event)
        {
            Database.Adresses = new Collection<string>();
        }
    }
}
namespace WebSample.ReadModel
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using Thering.EventSourced.Eventing;
    using Thering.EventSourced.Eventing.Handlers;

    using WebSample.Domain.User.Events;

    public class UserViewDenormalizer : IHandleEvent<UserAddressAdded>,
                                        IHandleEvent<UserCreated>
    {
        public void Handle(UserAddressAdded @event)
        {
            UserView.Adresses.Add(@event.Address);
            Trace.WriteLine("UserviewDenormalizer handled UserAddressAdded event !");
        }

        public void Handle(UserCreated @event)
        {
            UserView.Adresses = new Collection<string>();
        }
    }
}
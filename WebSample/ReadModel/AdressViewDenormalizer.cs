namespace WebSample.ReadModel
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using Thering.EventSourced.Eventing;
    using Thering.EventSourced.Eventing.Handlers;

    using WebSample.Domain.User.Events;

    public class AdressViewDenormalizer : IHandleEvent<UserAddressAdded>, IHandleEvent<UserCreated>
    {
        public void Handle(UserAddressAdded @event)
        {
            AdressView.Adresses.Add(@event.Address);
            Trace.WriteLine("AdressViewDenormalizer handled UserAddressAdded event !");
        }

        public void Handle(UserCreated @event)
        {
            AdressView.Adresses = new Collection<string>();
        }
    }
}
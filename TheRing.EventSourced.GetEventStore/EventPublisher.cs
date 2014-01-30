namespace TheRing.EventSourced.GetEventStore
{
    using System;
    using System.Collections.Generic;
    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;
    using Thering.EventSourced.Eventing;

    public class EventPublisher
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ISerializeEvent eventSerializer;
        private readonly Func<Type, IEnumerable<IEventQueue>> eventQueueFactory;

        public EventPublisher(IEventStoreConnection eventStoreConnection, ISerializeEvent eventSerializer, Func<Type, IEnumerable<IEventQueue>> eventQueueFactory)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.eventSerializer = eventSerializer;
            this.eventQueueFactory = eventQueueFactory;
            Subscribe();
        }

        private void Subscribe()
        {
            eventStoreConnection.SubscribeToAll(false, Publish, Disconnect, new UserCredentials("admin", "admin"));
        }

        private void Disconnect(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception exception)
        {
            Subscribe();
        }

        private void Publish(EventStoreSubscription subscription, ResolvedEvent @event)
        {
            if (!@event.OriginalStreamId.StartsWith("$")) // do not handle system event
            {
                var eventWithMetadata = eventSerializer.Deserialize(@event.OriginalEvent, @event.OriginalEventNumber);

                foreach (var eventQueue in eventQueueFactory(eventWithMetadata.Event.GetType()))
                {
                    eventQueue.Push(eventWithMetadata);
                }
            }
        }
    }
}
namespace TheRing.EventSourced.GetEventStore
{
    using System;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;

    using Thering.EventSourced.Eventing.Handlers;

    public class GetEventStoreEventPublisher
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ISerializeEvent eventSerializer;
        private readonly IEventQueue eventQueue;

        public GetEventStoreEventPublisher(IEventStoreConnection eventStoreConnection, ISerializeEvent eventSerializer, IEventQueue eventQueue)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.eventSerializer = eventSerializer;
            this.eventQueue = eventQueue;
            Subscribe();
        }

        private void Subscribe()
        {
            eventStoreConnection.SubscribeToStream("$AllAggregatesStream",true, Publish, Disconnect, new UserCredentials("admin", "admin"));
        }

        private void Disconnect(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception exception)
        {
            Subscribe();
        }

        private void Publish(EventStoreSubscription subscription, ResolvedEvent @event)
        {
            var eventWithMetadata = eventSerializer.Deserialize(@event.OriginalEvent);
            eventQueue.Push(eventWithMetadata, @event.OriginalEventNumber);
        }
    }
}
namespace TheRing.EventSourced.GetEventStore
{
    using System;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;

    using Thering.EventSourced.Eventing.Constants;
    using Thering.EventSourced.Eventing.Handlers;
    using Thering.EventSourced.Eventing.Repositories;

    using TheRing.EventSourced.GetEventStore.Serializers;

    public class GetEventStoreEventPublisher
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly IEventPositionRepository eventPositionRepository;
        private readonly ISerializeEvent eventSerializer;
        private readonly IEventQueue eventQueue;

        public GetEventStoreEventPublisher(IEventStoreConnection eventStoreConnection, IEventPositionRepository eventPositionRepository, ISerializeEvent eventSerializer, IEventQueue eventQueue)
        {
            this.eventStoreConnection = eventStoreConnection;
            this.eventPositionRepository = eventPositionRepository;
            this.eventSerializer = eventSerializer;
            this.eventQueue = eventQueue;
            Subscribe();
        }

        private void Subscribe()
        {
            var lastPositionToHandle = eventPositionRepository.GetlastPosition();
            eventStoreConnection.SubscribeToStreamFrom(StreamId.AllAggregatesStream, lastPositionToHandle, true, Publish, subscriptionDropped: Disconnect, userCredentials: new UserCredentials("admin", "changeit"));
        }

        private void Disconnect(
            EventStoreCatchUpSubscription subscription,
            SubscriptionDropReason dropReason,
            Exception exception)
        {
            //TODO gestion de reconnection
            subscription.Start();
        }

        private void Publish(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
        {
            var eventWithMetadata = eventSerializer.Deserialize(@event.OriginalEvent);
            eventQueue.Push(eventWithMetadata, @event.OriginalEventNumber);  
        }
    }
}
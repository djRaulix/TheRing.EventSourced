namespace TheRing.EventSourced.GetEventStore
{
    using System;

    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;

    using Thering.EventSourced.Eventing.Handlers;
    using Thering.EventSourced.Eventing.Repositories;

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
            var lastPositionToHandle = eventPositionRepository.GetMinUnhandledPosition();
            eventStoreConnection.SubscribeToStreamFrom("$AllAggregatesStream", lastPositionToHandle, true, Publish, Disconnect, userCredentials: new UserCredentials("admin", "admin"));
        }

        private void Disconnect(EventStoreCatchUpSubscription eventStoreSubscription)
        {
            Subscribe();
        }

        private void Publish(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
        {
            var eventWithMetadata = eventSerializer.Deserialize(@event.OriginalEvent);
            eventQueue.Push(eventWithMetadata, @event.OriginalEventNumber);
        }
    }
}
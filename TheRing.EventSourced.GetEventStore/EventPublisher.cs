﻿namespace TheRing.EventSourced.GetEventStore
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

        private void Disconnect(EventStoreSubscription arg1, SubscriptionDropReason arg2, Exception arg3)
        {
            Subscribe();
        }

        private void Publish(EventStoreSubscription subscription, ResolvedEvent @event)
        {
            if (!@event.OriginalStreamId.StartsWith("$")) // do not handle system event
            {
                var deserializedEvent = eventSerializer.Deserialize(@event.OriginalEvent).Event;
                
                foreach (var eventQueue in eventQueueFactory(deserializedEvent.GetType()))
                {
                    eventQueue.Push(deserializedEvent);
                }
            }
        }
    }
}
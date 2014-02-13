namespace TheRing.EventSourced.Redis
{
    using System;

    using ServiceStack.Redis;

    using Thering.EventSourced.Eventing.Handlers;

    public class RedisErrorHandler : IHandleError
    {
        private readonly IRedisClient redisClient;

        public RedisErrorHandler(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public void HandleError(object @event, int eventPosition, Type eventHandlerType, Exception exception)
        {
            this.redisClient.Set("urn:EventHandlingErrors:" + eventPosition, new ErrorDetail(@event, exception, eventHandlerType));
        }

        public class ErrorDetail
        {
            public readonly object Event;

            public readonly Exception Exception;

            public readonly Type EventHandlerType;

            public ErrorDetail(object @event, Exception exception, Type eventHandlerType)
            {
                this.Event = @event;
                this.Exception = exception;
                this.EventHandlerType = eventHandlerType;
            }
        }
    }
}
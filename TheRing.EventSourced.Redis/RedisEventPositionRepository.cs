namespace TheRing.EventSourced.Redis
{
    using System.Collections.Generic;

    using ServiceStack.Redis;

    using Thering.EventSourced.Eventing.Repositories;

    public class RedisEventPositionRepository : IEventPositionRepository
    {
        private readonly IRedisClient redisClient;
        private readonly IDictionary<int, int> positions = new Dictionary<int, int>();  

        public RedisEventPositionRepository(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public void Create(int eventPosition, int eventHandlerNumber)
        {
            redisClient.Set(GetEventPositionKey(eventPosition), eventHandlerNumber);
            redisClient.Save();
            positions.Add(eventPosition, eventHandlerNumber);
        }

        private static string GetEventPositionKey(int eventPosition)
        {
            return "EventPosition/" + eventPosition;
        }

        public void Decrement(int eventPosition)
        {
            positions[eventPosition] --;
            if (positions[eventPosition] == 0)
            {
                redisClient.Remove(GetEventPositionKey(eventPosition));
            }
            else
            {
                redisClient.Set(GetEventPositionKey(eventPosition), positions[eventPosition]);
              //  redisClient.Save();
            }
        }

    }
}
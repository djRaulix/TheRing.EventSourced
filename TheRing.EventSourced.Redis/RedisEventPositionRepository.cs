namespace TheRing.EventSourced.Redis
{
    using System.Collections.Generic;
    using System.Linq;

    using ServiceStack.Redis;

    using Thering.EventSourced.Eventing.Repositories;

    public class RedisEventPositionRepository : IEventPositionRepository
    {
        private readonly IRedisClient redisClient;
        private readonly IDictionary<int, int> positions = new Dictionary<int, int>();
        private const string LastUnhandledEventPosition = "LastUnhandledEventPosition";

        public RedisEventPositionRepository(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public void Create(int eventPosition, int eventHandlerCount)
        {
            if (!positions.Keys.Any() || positions.Keys.Min() > eventPosition)
            {
                this.UpdateLastUnhandledEventPositon(eventPosition);
            }

            positions.Add(eventPosition, eventHandlerCount);
        }

        public void Decrement(int eventPosition)
        {
            positions[eventPosition] --;
            if (positions[eventPosition] == 0)
            {
                positions.Remove(eventPosition);
                
                if (positions.Keys.Any() && positions.Keys.Min() > eventPosition)
                {
                    this.UpdateLastUnhandledEventPositon(positions.Keys.Min());
                }
            }
        }

        public int? GetMinUnhandledPosition()
        {
            return redisClient.Get<int?>(LastUnhandledEventPosition);
        }

        private void UpdateLastUnhandledEventPositon(int eventPosition)
        {
            this.redisClient.Set(LastUnhandledEventPosition, eventPosition);
            this.redisClient.Save();
        }
    }
}
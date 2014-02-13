namespace TheRing.EventSourced.Redis
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using ServiceStack.Redis;

    using Thering.EventSourced.Eventing.Repositories;

    public class RedisEventPositionRepository : IEventPositionRepository
    {
        private readonly IRedisClient redisClient;
        private const string LastUnhandledEventPosition = "urn:LastUnhandledEventPosition";

        private readonly BlockingCollection<int> queue = new BlockingCollection<int>();

        private readonly List<int> handledPositions = new List<int>();

        private int lastSavedPosition;

        public RedisEventPositionRepository(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
            this.lastSavedPosition = this.GetlastPosition() ?? -1;
            var waiter = new Thread(this.WaitAndHandle);
            waiter.Start();
        }


        private void WaitAndHandle()
        {
            while (!this.queue.IsCompleted)
            {
                var eventPosition = this.queue.Take();
                handledPositions.Add(eventPosition);
                var initialValue = lastSavedPosition;
                while (handledPositions.Any() && handledPositions.Min() - lastSavedPosition == 1)
                {
                    lastSavedPosition = handledPositions.Min();
                    handledPositions.Remove(handledPositions.Min());
                }
                if (initialValue != lastSavedPosition)
                {
                    this.redisClient.Set(LastUnhandledEventPosition, lastSavedPosition);
                    this.redisClient.SaveAsync();
                }
            }
        }

        public void Stop()
        {
            this.queue.CompleteAdding();
        }

        public int? GetlastPosition()
        {
            return redisClient.Get<int?>(LastUnhandledEventPosition);
        }

        public void SavePosition(int eventPosition)
        {
            if (!this.queue.IsAddingCompleted)
            {
                this.queue.Add(eventPosition);
            }
        }
    }
}
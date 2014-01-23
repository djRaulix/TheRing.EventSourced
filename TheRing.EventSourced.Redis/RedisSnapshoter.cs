namespace TheRing.EventSourced.Redis
{
    #region using

    using ServiceStack.Redis;

    using TheRing.EventSourced.Domain.Repository;
    using TheRing.EventSourced.Domain.Repository.Snapshot;

    #endregion

    public class RedisSnapshoter : ISnapshoter
    {
        #region Fields

        private readonly IRedisClient redis;

        #endregion

        #region Constructors and Destructors

        public RedisSnapshoter(IRedisClient redis)
        {
            this.redis = redis;
        }

        #endregion

        #region Public Methods and Operators

        public object Get(string key)
        {
            return this.redis.Get<object>(key);
        }

        public void Set(string key, object snapshot)
        {
            this.redis.Set(key, snapshot);
        }

        #endregion
    }
}
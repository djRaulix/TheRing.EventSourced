namespace TheRing.EventSourced.Domain.Snapshot
{
    #region using

    using System;
    using System.Collections.Concurrent;

    #endregion

    public class InMemorySnapshotKeeper : IKeepSnapshot
    {
        #region Fields

        private readonly ConcurrentDictionary<Guid, object> memory = new ConcurrentDictionary<Guid, object>();

        #endregion

        #region Public Methods and Operators

        public void Delete(Guid id)
        {
            object snapshot;
            this.memory.TryRemove(id, out snapshot);
        }

        public object Get(Guid id)
        {
            object snapshot;

            this.memory.TryGetValue(id, out snapshot);

            return snapshot;
        }

        public void Set(Guid id, object snapshot)
        {
            this.memory[id] = snapshot;
        }

        #endregion
    }
}
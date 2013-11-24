namespace TheRing.EventSourced.Domain.Repository.Snapshot
{
    public interface ISnapshoter
    {
        object Get(string key);

        void Set(string key, object snapshot);
    }
}
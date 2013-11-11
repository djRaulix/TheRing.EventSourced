namespace TheRing.EventSourced.Domain.Repository.Create
{
    using TheRing.EventSourced.Domain.Aggregate;

    public interface IAggregateRootFactory
    {
        T New<T>() where T : AggregateRoot;
    }
}
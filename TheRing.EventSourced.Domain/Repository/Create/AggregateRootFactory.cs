namespace TheRing.EventSourced.Domain.Repository.Create
{
    using TheRing.EventSourced.Core.Reflection.Activator;
    using TheRing.EventSourced.Domain.Aggregate;

    public class AggregateRootFactory : IAggregateRootFactory
    {
        public T New<T>() where T : AggregateRoot
        {
            return Activator<T>.CreateInstance();
        }
    }
}
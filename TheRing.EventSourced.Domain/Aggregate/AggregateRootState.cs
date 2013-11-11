namespace TheRing.EventSourced.Domain.Aggregate
{
    public abstract class AggregateRootState
    {
        #region Properties

        internal int Version { get; set; }

        #endregion

        #region Methods

        internal void When(object @event)
        {
        }

        #endregion
    }
}
namespace TheRing.EventSourced.Domain.Repository.Save
{
    #region using

    using System.Linq;

    using TheRing.EventSourced.Core;
    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class AggregateRootSaver : ISaveAggregateRoot
    {
        #region Fields

        private readonly ISAveEventsOnStream eventsSaver;

        private readonly IGetStreamNameFromAggregateRoot streamNameGetter;

        #endregion

        #region Constructors and Destructors

        public AggregateRootSaver(ISAveEventsOnStream eventsSaver, IGetStreamNameFromAggregateRoot streamNameGetter)
        {
            this.eventsSaver = eventsSaver;
            this.streamNameGetter = streamNameGetter;
        }

        #endregion

        #region Public Methods and Operators

        public void Save<TAgg>(TAgg aggregateRoot) where TAgg : AggregateRoot
        {
            this.eventsSaver.Save(
                this.streamNameGetter.Get(aggregateRoot), 
                aggregateRoot.Changes, 
                expectedVersion: aggregateRoot.Version - aggregateRoot.Changes.Count());
        }

        #endregion
    }
}
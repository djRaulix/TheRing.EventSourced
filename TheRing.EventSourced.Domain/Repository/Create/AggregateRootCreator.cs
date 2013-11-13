namespace TheRing.EventSourced.Domain.Repository.Create
{
    #region using

    using System;

    using TheRing.EventSourced.Core.Reflection.Activator;
    using TheRing.EventSourced.Domain.Aggregate;

    #endregion

    public class AggregateRootCreator : ICreateAggregateRoot
    {
        #region Public Methods and Operators

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var agg = Activator<TAgg>.CreateInstance();
            agg.Id = id;
            return agg;
        }

        #endregion
    }
}
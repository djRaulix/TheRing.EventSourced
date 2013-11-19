namespace TheRing.EventSourced.Application
{
    #region using

    using System;

    #endregion

    public abstract class CreateCommand
    {
        #region Fields

        private readonly Guid id = Guid.NewGuid();

        #endregion

        #region Public Properties

        public Guid AggregateRootId
        {
            get
            {
                return this.id;
            }
        }

        #endregion
    }
}
namespace TheRing.EventSourced.Application
{
    #region using

    using System;

    #endregion

    public abstract class UpdateCommand
    {
        
        #region Public Properties

        public Guid AggregateRootId { get; set; }

        public int ExpectedVersion { get; set; }

        #endregion
    }
}
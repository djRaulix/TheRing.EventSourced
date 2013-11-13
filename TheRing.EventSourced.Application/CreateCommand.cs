namespace TheRing.EventSourced.Application
{
    #region using

    using System;

    #endregion

    public abstract class CreateCommand
    {
        #region Fields

        private Guid id;

        #endregion

        #region Public Properties

        public Guid AggregateRootId
        {
            get
            {
                if (this.id == Guid.Empty)
                {
                    this.id = Guid.NewGuid();
                }

                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        #endregion
    }
}
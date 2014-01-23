namespace TheRing.Test.Fakes
{
    using System;

    public class FakeEvent
    {
        #region Fields

        public readonly Guid No;

        #endregion

        #region Constructors and Destructors

        public FakeEvent(Guid no)
        {
            this.No = no;
        }

        #endregion
    }
}
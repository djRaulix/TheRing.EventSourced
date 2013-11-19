namespace TheRing.Test
{
    #region using

    using NUnit.Framework;

    #endregion

    [TestFixture]
    public abstract class SpecBase
    {
        #region Public Methods and Operators

        [TestFixtureSetUp]
        public virtual void MainSetup()
        {
            this.EstablishContext();
            this.BecauseOf();
        }

        [TestFixtureTearDown]
        public virtual void MainTeardown()
        {
            this.Cleanup();
        }

        #endregion

        #region Methods

        protected virtual void BecauseOf()
        {
        }

        protected virtual void Cleanup()
        {
        }

        protected virtual void EstablishContext()
        {
        }

        #endregion
    }
}
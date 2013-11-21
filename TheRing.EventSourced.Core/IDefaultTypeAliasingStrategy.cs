namespace TheRing.EventSourced.Core
{
    using System;

    public interface IDefaultTypeAliasingStrategy
    {
        #region Public Methods and Operators

        string GetAlias(Type type);

        #endregion
    }
}
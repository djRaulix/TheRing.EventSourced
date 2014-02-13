namespace Thering.EventSourced.Eventing.Aliaser
{
    #region using

    using System;

    #endregion

    public interface ITypeAliaser
    {
        #region Public Methods and Operators

        Type GetType(string alias);

        string GetAlias(object @object);

        ITypeAliaser AddAlias(string alias, Type type);

        #endregion
    }
}
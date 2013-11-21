﻿namespace TheRing.EventSourced.Core
{
    #region using

    using System;

    #endregion

    public interface ITypeAliaser
    {
        #region Public Methods and Operators

        Type GetType(string alias);

        string GetAlias(Type type);

        string GetAlias(object @object);

        void AddAlias(string alias, Type type);

        #endregion
    }
}
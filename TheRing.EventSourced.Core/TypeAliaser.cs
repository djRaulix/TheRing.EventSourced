namespace TheRing.EventSourced.Core
{
    #region using

    using System;
    using System.Collections.Concurrent;

    #endregion

    public class TypeAliaser : ITypeAliaser
    {
        #region Fields

        private readonly ConcurrentDictionary<Type, string> aliases = new ConcurrentDictionary<Type, string>();

        private readonly IDefaultTypeAliasingStrategy defaultTypeAliasingStrategy;

        private readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

        #endregion

        #region Constructors and Destructors

        public TypeAliaser(IDefaultTypeAliasingStrategy defaultTypeAliasingStrategy)
        {
            this.defaultTypeAliasingStrategy = defaultTypeAliasingStrategy;
        }

        #endregion

        #region Public Methods and Operators

        public void AddAlias(string alias, Type type)
        {
            this.types[alias] = type;
            this.aliases[type] = alias;
        }

        public string GetAlias(object @object)
        {
            return this.GetAlias(@object.GetType());
        }

        public string GetAlias(Type type)
        {
            string alias;

            if (!this.aliases.TryGetValue(type, out alias))
            {
                alias = this.defaultTypeAliasingStrategy.GetAlias(type);
                this.AddAlias(alias, type);
            }

            return alias;
        }

        public Type GetType(string alias)
        {
            return this.types[alias];
        }

        #endregion
    }
}
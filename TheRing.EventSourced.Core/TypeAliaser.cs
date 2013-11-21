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

        private readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Methods and Operators

        public ITypeAliaser AddAlias(string alias, Type type)
        {
            this.types[alias] = type;
            this.aliases[type] = alias;
            return this;
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
                alias = type.AssemblyQualifiedName;
                this.AddAlias(alias, type);
            }

            return alias;
        }

        public Type GetType(string alias)
        {
            Type type;
            if (!this.types.TryGetValue(alias, out type))
            {
                type = Type.GetType(alias);
            }

            return type;
        }

        #endregion
    }
}
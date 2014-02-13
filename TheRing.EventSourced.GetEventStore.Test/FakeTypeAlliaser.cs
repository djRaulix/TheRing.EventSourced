namespace TheRing.EventSourced.GetEventStore.Test
{
    using System;
    using System.Collections.Concurrent;

    using Thering.EventSourced.Eventing.Aliaser;
    using Thering.EventSourced.Eventing.Constants;
    using Thering.EventSourced.Eventing.Repositories;

    public class FakeTypeAlliaser : ITypeAliaser
    {
        #region Fields

        private readonly ConcurrentDictionary<Type, string> aliases = new ConcurrentDictionary<Type, string>();

        private readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

        private readonly IEventStreamRepository eventStreamRepository;

        #endregion

        #region Constructors and Destructors

        public FakeTypeAlliaser(IEventStreamRepository eventStreamRepository)
        {
            this.eventStreamRepository = eventStreamRepository;

        }

        #endregion

        #region Public Methods and Operators

        public ITypeAliaser AddAlias(string alias, Type type)
        {
            this.types[alias] = type;
            this.aliases[type] = alias;
            this.eventStreamRepository.Save(StreamId.EventTypesStream, type);
            return this;
        }

        public string GetAlias(object @object)
        {
            return this.GetAlias(@object.GetType());
        }

        private string GetAlias(Type type)
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
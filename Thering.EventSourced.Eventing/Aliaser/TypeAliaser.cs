namespace Thering.EventSourced.Eventing.Aliaser
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using Thering.EventSourced.Eventing.Constants;
    using Thering.EventSourced.Eventing.Repositories;

    public class TypeAliaser : ITypeAliaser
    {
        #region Fields

        private readonly ConcurrentDictionary<Type, string> aliases = new ConcurrentDictionary<Type, string>();

        private readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

        private readonly IEventStreamRepository eventStreamRepository;

        #endregion

        #region Constructors and Destructors

        public TypeAliaser(IEventStreamRepository eventStreamRepository)
        {
            this.eventStreamRepository = eventStreamRepository;
            
            foreach (var eventType in this.eventStreamRepository.Get(StreamId.EventTypesStream).Where(e => e != null).Cast<Type>())
            {
                this.types[eventType.AssemblyQualifiedName] = eventType;
                this.aliases[eventType] = eventType.AssemblyQualifiedName;
            }
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
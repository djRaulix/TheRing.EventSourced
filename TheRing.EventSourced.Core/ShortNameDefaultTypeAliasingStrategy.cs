namespace TheRing.EventSourced.Core
{
    using System;

    public class ShortNameDefaultTypeAliasingStrategy : IDefaultTypeAliasingStrategy
    {
        public string GetAlias(Type type)
        {
            return type.Name;
        }
    }
}
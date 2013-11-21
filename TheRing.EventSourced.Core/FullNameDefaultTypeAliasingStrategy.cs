namespace TheRing.EventSourced.Core
{
    using System;

    public class FullNameDefaultTypeAliasingStrategy : IDefaultTypeAliasingStrategy
    {
        public string GetAlias(Type type)
        {
            return type.FullName;
        }
    }
}
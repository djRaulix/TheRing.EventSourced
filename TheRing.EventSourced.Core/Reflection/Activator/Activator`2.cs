namespace TheRing.EventSourced.Core.Reflection.Activator
{
    using System;

    public static class Activator<T, TArg1> where T : class
    {
        public static Func<TArg1, T> CreateInstance { get; private set; }

        static Activator()
        {
            CreateInstance = (Func<TArg1, T>)typeof(T).GetConstructorDelegate(typeof(Func<TArg1, T>));
        }
    }
}
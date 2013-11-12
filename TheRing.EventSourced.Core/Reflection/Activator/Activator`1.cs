using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.EventSourced.Core.Reflection.Activator
{
    public static class Activator<T> where T : class
    {
        public static Func<T> CreateInstance { get; private set; }

        static Activator()
        {
            CreateInstance = typeof(T).GetConstructorDelegate<T>();
        }
    }
}

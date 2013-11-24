﻿namespace TheRing.EventSourced.Core.Log
{
    using System;

    public static class LogManager
    {

        private static Func<string, ILogger> _logFactory = x => new ConsoleLogger(x);

        public static ILogger GetLoggerFor(Type type)
        {
            return GetLogger(type.Name);
        }

        public static ILogger GetLoggerFor<T>()
        {
            return GetLogger(typeof(T).Name);
        }

        public static ILogger GetLogger(string logName)
        {
            return new LazyLogger(() => _logFactory(logName));
        }

        public static void SetLogFactory(Func<string, ILogger> factory)
        {
            _logFactory = factory;
        }
    }
}
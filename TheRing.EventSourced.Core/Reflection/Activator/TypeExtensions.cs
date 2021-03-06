﻿namespace TheRing.EventSourced.Core.Reflection.Activator
{
    #region using

    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    #endregion

    public static class TypeExtensions
    {
        #region Static Fields

        private static readonly ConcurrentDictionary<Type, Func<object>> constructors =
            new ConcurrentDictionary<Type, Func<object>>();

        #endregion

        #region Public Methods and Operators

        public static object CreateInstance(this Type type)
        {
            Func<object> constructor;
            if (!constructors.TryGetValue(type, out constructor))
            {
                constructor = type.GetConstructorDelegate();
                constructors.TryAdd(type, constructor);
            }

            return constructor();
        }

        public static object CreateInstance<TArg1>(this Type type, TArg1 arg1)
        {
            return PrivateActivator<TArg1>.CreateInstance(type, arg1);
        }

        public static Func<TBase> GetConstructorDelegate<TBase>(this Type type)
        {
            return (Func<TBase>)GetConstructorDelegate(type, typeof(Func<TBase>));
        }

        public static Func<object> GetConstructorDelegate(this Type type)
        {
            return (Func<object>)GetConstructorDelegate(type, typeof(Func<object>));
        }

        public static Delegate GetConstructorDelegate(this Type type, Type delegateType)
        {
            if (delegateType == null)
            {
                throw new ArgumentNullException("delegateType");
            }

            Type[] genericArguments = delegateType.GetGenericArguments();
            Type[] argTypes = genericArguments.Length > 1
                                  ? genericArguments.Take(genericArguments.Length - 1).ToArray()
                                  : Type.EmptyTypes;

            ConstructorInfo constructor = type.GetConstructor(argTypes);
            if (constructor == null)
            {
                if (argTypes.Length == 0)
                {
                    throw new InvalidProgramException(
                        string.Format("Type '{0}' doesn't have a parameterless constructor.", type.Name));
                }

                throw new InvalidProgramException(
                    string.Format("Type '{0}' doesn't have the requested constructor.", type.Name));
            }

            var dynamicMethod = new DynamicMethod("DM$_" + type.Name, type, argTypes, type);
            ILGenerator ilGen = dynamicMethod.GetILGenerator();
            for (int i = 0; i < argTypes.Length; i++)
            {
                ilGen.Emit(OpCodes.Ldarg, i);
            }

            ilGen.Emit(OpCodes.Newobj, constructor);
            ilGen.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(delegateType);
        }

        #endregion

        private static class PrivateActivator<TArg1>
        {
            #region Static Fields

            private static readonly ConcurrentDictionary<Type, Func<TArg1, object>> _constructors =
                new ConcurrentDictionary<Type, Func<TArg1, object>>();

            #endregion

            #region Public Methods and Operators

            public static object CreateInstance(Type type, TArg1 arg1)
            {
                Func<TArg1, object> constructor;
                if (!_constructors.TryGetValue(type, out constructor))
                {
                    constructor = (Func<TArg1, object>)type.GetConstructorDelegate(typeof(Func<TArg1, object>));
                    _constructors.TryAdd(type, constructor);
                }

                return constructor(arg1);
            }

            #endregion
        }
    }
}
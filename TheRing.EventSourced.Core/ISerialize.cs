namespace TheRing.EventSourced.Core
{
    using System;

    public interface ISerialize
    {
        byte[] Serialize(object obj);

        object Deserialize(byte[] data, Type type);

        T Deserialize<T>(byte[] data);
    }
}
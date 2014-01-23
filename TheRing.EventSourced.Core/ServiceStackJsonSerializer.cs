namespace TheRing.EventSourced.Core
{
    #region using

    using System;
    using System.Text;

    using ServiceStack.Text;

    #endregion

    public class ServiceStackJsonSerializer : ISerialize
    {
        public ServiceStackJsonSerializer()
        {
            JsConfig.IncludePublicFields = true;
        }
        
        #region Public Methods and Operators

        public object Deserialize(byte[] data, Type type)
        {
            return JsonSerializer.DeserializeFromString(data.FromUtf8Bytes(), type);
        }

        public T Deserialize<T>(byte[] data)
        {
            return JsonSerializer.DeserializeFromString<T>(data.FromUtf8Bytes());
        }

        public byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonSerializer.SerializeToString(obj).ToUtf8Bytes();
        }

        #endregion
    }
}
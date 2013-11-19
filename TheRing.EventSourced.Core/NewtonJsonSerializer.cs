namespace TheRing.EventSourced.Core
{
    #region using

    using System;
    using System.Text;

    using Newtonsoft.Json;

    #endregion

    public class NewtonJsonSerializer : ISerialize
    {
        #region Fields

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
                                                                         {
                                                                             TypeNameHandling =
                                                                                 TypeNameHandling
                                                                                 .None
                                                                         };

        #endregion

        #region Public Methods and Operators

        public object Deserialize(byte[] data, Type type)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), type);
        }

        public T Deserialize<T>(byte[] data)
        {
            return (T)this.Deserialize(data, typeof(T));
        }

        public byte[] Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, this.serializerSettings));
        }

        #endregion
    }
}
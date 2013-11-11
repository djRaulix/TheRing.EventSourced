namespace TheRing.EventSourced.GetEventStore.Json
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Text;

    using EventStore.ClientAPI;

    using Magnum;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion

    public class NewtonJsonSerializer : ITransformToEventData, IGetEventFromRecorded
    {
        #region Constants

        private const string EventClrTypeHeader = "EventClrTypeName";

        #endregion

        #region Fields

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
                                                                         {
                                                                             TypeNameHandling =
                                                                                 TypeNameHandling
                                                                                 .None
                                                                         };

        #endregion

        #region Public Methods and Operators

        public object Get(RecordedEvent recordedEvent)
        {
            var eventClrTypeName =
                JObject.Parse(Encoding.UTF8.GetString(recordedEvent.Metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(
                Encoding.UTF8.GetString(recordedEvent.Data), 
                Type.GetType((string)eventClrTypeName));
        }

        public EventData Transform(object @event, IDictionary<string, object> headers = null)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, this.serializerSettings));

            var eventHeaders = headers == null
                                   ? new Dictionary<string, object>()
                                   : new Dictionary<string, object>(headers);

            eventHeaders.Add(EventClrTypeHeader, @event.GetType().AssemblyQualifiedName);

            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, this.serializerSettings));
            var typeName = @event.GetType().Name;

            return new EventData(CombGuid.Generate(), typeName, true, data, metadata);
        }

        #endregion
    }
}
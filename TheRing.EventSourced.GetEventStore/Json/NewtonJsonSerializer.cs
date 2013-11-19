namespace TheRing.EventSourced.GetEventStore.Json
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Text;

    using EventStore.ClientAPI;

    using Newtonsoft.Json;

    using Thering.EventSourced.Eventing;

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

        public EventWithMetadata Get(RecordedEvent recordedEvent)
        {
            var eventHeaders =
                (Dictionary<string, object>)
                JsonConvert.DeserializeObject(
                    Encoding.UTF8.GetString(recordedEvent.Metadata), 
                    typeof(Dictionary<string, object>));

            return
                new EventWithMetadata(
                    JsonConvert.DeserializeObject(
                        Encoding.UTF8.GetString(recordedEvent.Data), 
                        Type.GetType((string)eventHeaders[EventClrTypeHeader])), 
                    eventHeaders);
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

            return new EventData(Guid.NewGuid(), typeName, true, data, metadata);
        }

        #endregion
    }
}
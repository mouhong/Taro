using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Taro.Persistence.Serialization
{
    public class JsonEventSerializer : IEventSerializer<string>
    {
        private JsonSerializerSettings _serializerSettings;

        public JsonEventSerializer() : this(null) { }

        public JsonEventSerializer(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public string Serialize(IEvent theEvent)
        {
            return JsonConvert.SerializeObject(theEvent, typeof(IEvent), _serializerSettings);
        }

        public IEvent Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IEvent>(json, _serializerSettings);
        }
    }
}

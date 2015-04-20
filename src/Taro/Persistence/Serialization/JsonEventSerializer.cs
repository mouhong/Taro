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
        public string Serialize(IEvent theEvent)
        {
            return JsonConvert.SerializeObject(theEvent, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public IEvent Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IEvent>(json);
        }
    }
}

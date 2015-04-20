using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Persistence.Serialization;

namespace Taro.Persistence.RavenDB
{
    public class StoredEvent : IStoredEvent
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public DateTimeOffset UtcCreationTime { get; set; }
    }
}

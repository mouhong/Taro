using System;
using Taro.Persistence;

namespace Taro.RavenDB
{
    public class StoredEvent : IStoredEvent
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public DateTimeOffset UtcCreationTime { get; set; }
    }
}

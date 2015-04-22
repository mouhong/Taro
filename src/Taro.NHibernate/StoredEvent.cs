using System;
using Taro.Persistence;

namespace Taro.NHibernate
{
    public class StoredEvent : IStoredEvent
    {
        public virtual Guid Id { get; set; }

        public virtual string Body { get; set; }

        public virtual DateTimeOffset UtcCreationTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Taro
{
    [Serializable, DataContract]
    public abstract class Event : IEvent
    {
        [DataMember]
        public DateTime UtcTimestamp { get; protected set; }

        protected Event()
            : this(DateTime.UtcNow)
        {
        }

        protected Event(DateTime utcTimestamp)
        {
            UtcTimestamp = utcTimestamp;
        }
    }
}

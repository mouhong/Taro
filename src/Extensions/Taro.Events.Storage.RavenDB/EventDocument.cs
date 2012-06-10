using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Storage.RavenDB
{
    public class EventDocument
    {
        public int Id { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public object Event { get; set; }

        public EventDocument()
            : this(DateTime.UtcNow)
        {
        }

        public EventDocument(DateTime utcTimeStamp)
        {
            UtcTimestamp = utcTimeStamp;
        }
    }
}

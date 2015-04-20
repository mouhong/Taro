using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;

namespace Taro
{
    public abstract class AggregateRoot : IEventSource
    {
        private EventStream _events = new EventStream();

        EventStream IEventSource.Events
        {
            get
            {
                return _events;
            }
        }

        protected virtual void AppendEvent(IEvent theEvent)
        {
            _events.Append(theEvent);
        }
    }
}

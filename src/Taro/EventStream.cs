﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public class EventStream : IEnumerable<IEvent>
    {
        private List<IEvent> _events;

        public EventStream()
        {
            _events = new List<IEvent>();
        }

        public void Append(IEvent @event)
        {
            _events.Add(@event);
        }

        public IEnumerator<IEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

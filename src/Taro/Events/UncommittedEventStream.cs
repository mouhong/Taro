using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events
{
    public class UncommittedEventStream : IEnumerable<IEvent>
    {
        private List<IEvent> _events = new List<IEvent>();

        public int Count
        {
            get
            {
                return _events.Count;
            }
        }

        public UncommittedEventStream()
        {
        }

        public void Append(IEvent evnt)
        {
            _events.Add(evnt);
        }

        public void Clear()
        {
            _events.Clear();
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

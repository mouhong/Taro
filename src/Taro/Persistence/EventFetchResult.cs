using System.Collections.Generic;
using System.Linq;

namespace Taro.Persistence
{
    public class EventFetchResult
    {
        public IList<IStoredEvent> Events { get; private set; }

        public bool HasMore { get; private set; }

        public EventFetchResult(IEnumerable<IStoredEvent> events, int batchSize)
        {
            Events = events.ToList();
            HasMore = Events.Count == batchSize;
        }

        public EventFetchResult(IEnumerable<IStoredEvent> events, bool hasMore)
        {
            Events = events.ToList();
            HasMore = hasMore;
        }
    }
}

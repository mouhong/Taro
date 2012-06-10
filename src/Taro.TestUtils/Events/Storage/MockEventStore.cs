using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events.Storage;

namespace Taro.TestUtils.Events.Storage
{
    public class MockEventStore : IEventStore
    {
        public Action<IEnumerable<IEvent>> SaveEventsAction { get; set; }

        public MockEventStore()
        {
        }

        public MockEventStore(Action<IEnumerable<IEvent>> saveEventsAction)
        {
            SaveEventsAction = saveEventsAction;
        }

        public IEnumerable<IEvent> LoadEvents(int fromId, int toId)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<IEvent> LoadEvents(DateTime? fromUtcTimestamp, DateTime? toUtcTimestamp)
        {
            throw new NotSupportedException();
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            if (SaveEventsAction != null)
            {
                SaveEventsAction(events);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Storage
{
    // TODO: Event store is not so useful. This can be moved to an extension which wraps the UnitOfWork, for example, EventStoreEnabledUnitOfWork.
    public interface IEventStore
    {
        IEnumerable<IEvent> LoadEvents(int fromId, int toId);

        IEnumerable<IEvent> LoadEvents(DateTime? fromUtcTimestamp, DateTime? toUtcTimestamp);

        void SaveEvents(IEnumerable<IEvent> events);
    }

    public class NullEventStore : IEventStore
    {
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Persistence
{
    public interface IDomainDbSession : IDisposable
    {
        void SaveAggregate<T>(T aggregate) where T : AggregateRoot;

        void AddEvents(IEnumerable<IEvent> events);

        EventFetchResult FetchEvents(int batchSize);

        void DeleteEvent(IStoredEvent storedEvent);

        IEvent UnwrapEvent(IStoredEvent storedEvent);

        void Commit();
    }
}

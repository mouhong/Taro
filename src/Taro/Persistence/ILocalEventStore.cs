using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Persistence
{
    public interface ILocalEventStore
    {
        IList<IStoredEvent> NextBatch(int batchSize);

        void Delete(IStoredEvent storedEvent);

        IEvent Unwrap(IStoredEvent storedEvent);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Persistence
{
    public interface ILocalEventStore
    {
        IEnumerable<IStoredEvent> Enumerate();

        void Delete(IStoredEvent @event);
    }
}

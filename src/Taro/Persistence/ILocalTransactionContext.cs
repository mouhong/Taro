using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Persistence
{
    public interface ILocalTransactionContext : IDisposable
    {
        void AddEvents(IEnumerable<IEvent> events);

        void Commit();
    }
}

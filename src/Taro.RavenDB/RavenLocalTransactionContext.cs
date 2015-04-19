using Raven.Client;
using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.Persistence.RavenDB
{
    public class RavenLocalTransactionContext : ILocalTransactionContext
    {
        private IDocumentSession _session;

        public RavenLocalTransactionContext(IDocumentSession session)
        {
            _session = session;
        }

        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _session.Store(new StoredEvent
                {
                    Body = JsonConvert.SerializeObject(@event),
                    UtcCreationTime = @event.UtcCreationTime
                });
            }
        }

        public void Commit()
        {
            _session.SaveChanges();
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}

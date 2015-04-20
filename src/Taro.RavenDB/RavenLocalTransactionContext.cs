using Raven.Client;
using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Persistence.Serialization;

namespace Taro.Persistence.RavenDB
{
    public class RavenLocalTransactionContext : ILocalTransactionContext
    {
        private IDocumentSession _session;
        private IEventSerializer<string> _serializer;
        private bool _ownedSession;

        public RavenLocalTransactionContext(IDocumentSession session, bool ownedSession)
        {
            _session = session;
            _serializer = new JsonEventSerializer();
            _ownedSession = ownedSession;
        }

        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var theEvent in events)
            {
                _session.Store(new StoredEvent
                {
                    Body = _serializer.Serialize(theEvent),
                    UtcCreationTime = theEvent.UtcCreationTime
                });
            }
        }

        public void Commit()
        {
            _session.SaveChanges();
        }

        public void Dispose()
        {
            if (_ownedSession)
            {
                _session.Dispose();
            }
        }
    }
}

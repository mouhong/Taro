using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::NHibernate;
using Taro.Persistence.Serialization;

namespace Taro.Persistence.NHibernate
{
    public class NhLocalTransactionContext : ILocalTransactionContext
    {
        private ISession _session;
        private ITransaction _transaction;
        private IEventSerializer<string> _serializer;
        private bool _ownedSession;

        public NhLocalTransactionContext(ISession session, ITransaction transaction, bool ownedSession)
        {
            _session = session;
            _transaction = transaction;
            _ownedSession = ownedSession;
            _serializer = new JsonEventSerializer();
        }

        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var theEvent in events)
            {
                var storedEvent = new StoredEvent
                {
                    Id = Guid.NewGuid(),
                    Body = _serializer.Serialize(theEvent),
                    UtcCreationTime = theEvent.UtcCreationTime
                };

                _session.Save(theEvent);
            }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();

            if (_ownedSession)
            {
                _session.Dispose();
            }
        }
    }
}

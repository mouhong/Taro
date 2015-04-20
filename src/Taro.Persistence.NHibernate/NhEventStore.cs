using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence.Serialization;

namespace Taro.Persistence.NHibernate
{
    public class NhEventStore : ILocalEventStore
    {
        private ISessionFactory _sessionFactory;
        private IEventSerializer<string> _serializer;

        public NhEventStore(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _serializer = new JsonEventSerializer();
        }

        public IEnumerable<IStoredEvent> Enumerate()
        {
            throw new NotImplementedException();
        }

        public void Delete(IStoredEvent storedEvent)
        {
            var eventId = ((StoredEvent)storedEvent).Id;

            using(var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var theEvent = session.Load<StoredEvent>(eventId);
                session.Delete(theEvent);
                tx.Commit();
            }
        }

        public IEvent Unwrap(IStoredEvent storedEvent)
        {
            var theEvent = (StoredEvent)storedEvent;
            return _serializer.Deserialize(theEvent.Body);
        }
    }
}

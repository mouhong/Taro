using NHibernate;
using NHibernate.Linq;
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

        public IList<IStoredEvent> NextBatch(int batchSize)
        {
            using (var session = _sessionFactory.OpenStatelessSession())
            {
                var result = new List<IStoredEvent>();
                var events = session.Query<StoredEvent>()
                                    .OrderByDescending(it => it.UtcCreationTime)
                                    .Take(batchSize)
                                    .ToList();

                result.AddRange(events);

                return result;
            }
        }

        public void Delete(IStoredEvent storedEvent)
        {
            var eventId = ((StoredEvent)storedEvent).Id;

            using (var session = _sessionFactory.OpenSession())
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

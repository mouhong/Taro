using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Persistence.Serialization;

namespace Taro.Persistence.RavenDB
{
    public class RavenEventStore : ILocalEventStore
    {
        private IDocumentStore _store;
        private IEventSerializer<string> _serializer;

        public RavenEventStore(IDocumentStore store)
        {
            _store = store;
            _serializer = new JsonEventSerializer();
        }

        public IEnumerable<IStoredEvent> Enumerate()
        {
            var session = _store.OpenSession();
            var query = session.Query<StoredEvent>()
                               .OrderByDescending(it => it.UtcCreationTime);
            
            var stream = _store.OpenSession().Advanced.Stream<StoredEvent>(query);

            while (stream.MoveNext())
            {
                yield return stream.Current.Document;
            }
        }

        public void Delete(IStoredEvent @event)
        {
            var id = ((StoredEvent)@event).Id;
            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
        }

        public IEvent Unwrap(IStoredEvent storedEvent)
        {
            return _serializer.Deserialize(((StoredEvent)storedEvent).Body);
        }
    }
}

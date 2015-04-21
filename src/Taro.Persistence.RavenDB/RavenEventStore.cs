using Raven.Client;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Persistence.RavenDB.Indexes;
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

            IndexCreation.CreateIndexes(typeof(RavenEventStore).Assembly, store);
        }

        public IList<IStoredEvent> NextBatch(int batchSize)
        {
            var session = _store.OpenSession();
            var query = session.Query<StoredEvent, StoredEventIndex>()
                               .OrderByDescending(it => it.UtcCreationTime);

            var result = new List<IStoredEvent>();

            using (var stream = _store.OpenSession().Advanced.Stream<StoredEvent>(query))
            {
                while (stream.MoveNext())
                {
                    result.Add(stream.Current.Document);

                    if (result.Count == batchSize)
                    {
                        break;
                    }
                }
            }

            return result;
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

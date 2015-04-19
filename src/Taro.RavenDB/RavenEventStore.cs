using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.Persistence.RavenDB
{
    public class RavenEventStore : ILocalEventStore
    {
        private IDocumentStore _store;

        public RavenEventStore(IDocumentStore store)
        {
            _store = store;
        }

        public IEnumerable<IStoredEvent> Enumerate()
        {
            // TODO: Change to ravendb streaming
            return _store.OpenSession().Query<StoredEvent>().ToList();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Taro.Events.Storage.RavenDB.Indexes;
using Taro.Utils;

namespace Taro.Events.Storage.RavenDB
{
    public class RavenEventStore : IEventStore
    {
        private IDocumentStore _documentStore;

        public RavenEventStore(IDocumentStore documentStore)
        {
            Require.NotNull(documentStore, "documentStore");
            _documentStore = documentStore;
        }

        public IEnumerable<IEvent> LoadEvents(int fromId, int toId)
        {
            using (var session = _documentStore.OpenSession())
            {
                var events = session.Query<EventDocument, EventDocumentIndex>()
                                    .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                    .Where(it => it.Id >= fromId && it.Id < toId)
                                    .OrderBy(e => e.UtcTimestamp);

                return events.Select(e => e.Event).ToList().Cast<IEvent>();
            }
        }

        public IEnumerable<IEvent> LoadEvents(DateTime? fromUtcTimestamp, DateTime? toUtcTimestamp)
        {
            using (var session = _documentStore.OpenSession())
            {
                IQueryable<EventDocument> events = session.Query<EventDocument, EventDocumentIndex>()
                                                          .Customize(x => x.WaitForNonStaleResultsAsOfNow());

                if (fromUtcTimestamp != null)
                {
                    events = events.Where(e => e.UtcTimestamp >= fromUtcTimestamp.Value);
                }
                if (toUtcTimestamp != null)
                {
                    events = events.Where(e => e.UtcTimestamp < toUtcTimestamp.Value);
                }

                return events.OrderBy(e => e.UtcTimestamp).Select(e => e.Event).ToList().Cast<IEvent>();
            }
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            using (var session = _documentStore.OpenSession())
            {
                foreach (var evnt in events)
                {
                    var document = new EventDocument
                    {
                        UtcTimestamp = evnt.UtcTimestamp,
                        Event = evnt
                    };

                    session.Store(document);
                }

                session.SaveChanges();
            }
        }
    }
}

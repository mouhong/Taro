using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using Taro.Persistence;
using Taro.Persistence.Serialization;
using Taro.RavenDB.Persistence.Indexes;

namespace Taro.RavenDB.Persistence
{
    public class RavenDomainDbSession : IDomainDbSession
    {
        private IDocumentSession _session;
        private IEventSerializer<string> _serializer;

        public IDocumentSession Session
        {
            get { return _session; }
        }

        public RavenDomainDbSession(IDocumentSession session)
        {
            _session = session;
            _serializer = new JsonEventSerializer();
        }

        public void SaveAggregate<T>(T aggregate) where T : AggregateRoot
        {
            _session.Store(aggregate);
        }

        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var eachEvent in events)
            {
                _session.Store(new StoredEvent
                {
                    Body = _serializer.Serialize(eachEvent),
                    UtcCreationTime = eachEvent.UtcCreationTime
                });
            }
        }

        /// <summary>
        /// Fetch events using the specified batch size from RavenDB.
        /// RavenDB is 'safe by default', so you we have to either use a batch size less than or equal to maximum page size specified in RavenDB, 
        /// or change RavenDB maximum page size to a bigger value.
        /// </summary>
        public EventFetchResult FetchEvents(int batchSize)
        {
            RavenQueryStatistics statistics;
            var events = _session.Query<StoredEvent, StoredEventIndex>()
                                 .Customize(it => it.WaitForNonStaleResultsAsOfNow())
                                 .Statistics(out statistics)
                                 .OrderByDescending(it => it.UtcCreationTime)
                                 .Take(batchSize)
                                 .ToList();

            var hasMore = false;

            if (statistics.TotalResults > events.Count)
            {
                hasMore = true;
            }

            return new EventFetchResult(events, hasMore);
        }

        public void DeleteEvent(IStoredEvent theEvent)
        {
            var id = ((StoredEvent)theEvent).Id;
            _session.Delete(id);
        }

        public IEvent UnwrapEvent(IStoredEvent storedEvent)
        {
            return _serializer.Deserialize(((StoredEvent)storedEvent).Body);
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

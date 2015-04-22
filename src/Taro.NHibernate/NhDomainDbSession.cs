using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using Taro.Persistence;
using Taro.Persistence.Serialization;

namespace Taro.NHibernate
{
    public class NhDomainDbSession : IDomainDbSession
    {
        private ISession _session;
        private IEventSerializer<string> _serializer;

        public ISession Session
        {
            get { return _session; }
        }

        public NhDomainDbSession(ISession session)
        {
            Require.NotNull(session, "session");

            _session = session;
            _serializer = new JsonEventSerializer();
        }

        public EventFetchResult FetchEvents(int batchSize)
        {
            var result = new List<IStoredEvent>();
            var events = _session.Query<StoredEvent>()
                                 .OrderByDescending(it => it.UtcCreationTime)
                                 .Take(batchSize)
                                 .ToList();

            result.AddRange(events);

            return new EventFetchResult(result, batchSize);
        }

        public void DeleteEvent(IStoredEvent storedEvent)
        {
            var eventId = ((StoredEvent)storedEvent).Id;
            var theEvent = _session.Load<StoredEvent>(eventId);
            _session.Delete(theEvent);
        }

        public IEvent UnwrapEvent(IStoredEvent storedEvent)
        {
            var theEvent = (StoredEvent)storedEvent;
            return _serializer.Deserialize(theEvent.Body);
        }

        public void SaveAggregate<T>(T aggregate) where T : AggregateRoot
        {
            _session.Save(aggregate);
        }

        public void AddEvents(IEnumerable<IEvent> events)
        {
            foreach (var eachEvent in events)
            {
                _session.Save(new StoredEvent
                {
                    Body = _serializer.Serialize(eachEvent),
                    UtcCreationTime = eachEvent.UtcCreationTime
                });
            }
        }

        public void Commit()
        {
            _session.Flush();
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}

using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using System;
using System.Linq;
using Taro.Transports;

namespace Taro.Persistence.RavenDB
{
    public class RavenDomainRepository : DomainRepositoryBase
    {
        private IDocumentSession _session;

        public IDocumentSession Session
        {
            get { return _session; }
        }

        public RavenDomainRepository(IDocumentSession session, IEventBus eventBus, IRelayWorker relayWorker)
            : base(eventBus, relayWorker)
        {
            _session = session;
        }

        public override T Find<T>(object id)
        {
            if (id is String)
            {
                return _session.Load<T>((String)id);
            }
            if (id is ValueType)
            {
                return _session.Load<T>((ValueType)id);
            }

            throw new ArgumentException("Invalid id type. Expects string or value type.");
        }

        protected override void SaveWithoutCommit<T>(T aggregate)
        {
            _session.Store(aggregate);
        }

        protected override void DeleteWithoutCommit<T>(T aggregate)
        {
            _session.Delete(aggregate);
        }

        protected override ILocalTransactionContext CreateLocalTransactionContext()
        {
            return new RavenLocalTransactionContext(_session, false);
        }

        public override void Dispose()
        {
            _session.Dispose();
        }
    }
}

using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Workers;

namespace Taro.RavenDB
{
    public class RavenDomainRepositoryFactory : IDomainRepositoryFactory
    {
        private IDocumentStore _store;
        private Lazy<IRelayWorker> _relayWorker;

        public RavenDomainRepositoryFactory(IDocumentStore store, Func<IRelayWorker> relayWorkerAccessor)
        {
            Require.NotNull(store, "store");
            Require.NotNull(relayWorkerAccessor, "relayWorkerAccessor");

            _store = store;
            _relayWorker = new Lazy<IRelayWorker>(relayWorkerAccessor, true);
        }

        public IDomainRepository CreateDomainRepository()
        {
            return new RavenDomainRepository(_store.OpenSession(), _relayWorker.Value);
        }
    }
}

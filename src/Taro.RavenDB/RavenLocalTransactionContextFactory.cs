using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.Persistence.RavenDB
{
    public class RavenLocalTransactionContextFactory : ILocalTransactionContextFactory
    {
        private IDocumentStore _store;

        public RavenLocalTransactionContextFactory(IDocumentStore store)
        {
            _store = store;
        }

        public ILocalTransactionContext CreateLocalTransactionContext()
        {
            return new RavenLocalTransactionContext(_store.OpenSession());
        }
    }
}

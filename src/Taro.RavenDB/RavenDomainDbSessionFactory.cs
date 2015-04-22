using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.RavenDB
{
    public class RavenDomainDbSessionFactory : IDomainDbSessionFactory
    {
        private IDocumentStore _store;

        public RavenDomainDbSessionFactory(IDocumentStore store)
        {
            _store = store;
        }

        public IDomainDbSession OpenSession()
        {
            return new RavenDomainDbSession(_store.OpenSession());
        }
    }
}

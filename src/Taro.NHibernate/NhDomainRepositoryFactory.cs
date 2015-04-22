using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Workers;

namespace Taro.NHibernate
{
    public class NhDomainRepositoryFactory : IDomainRepositoryFactory
    {
        private ISessionFactory _sessionFactory;
        private Lazy<IRelayWorker> _relayWorker;

        public NhDomainRepositoryFactory(ISessionFactory sessionFactory, Func<IRelayWorker> relayWorkerAccessor)
        {
            Require.NotNull(sessionFactory, "sessionFactory");
            Require.NotNull(relayWorkerAccessor, "relayWorkerAccessor");

            _sessionFactory = sessionFactory;
            _relayWorker = new Lazy<IRelayWorker>(relayWorkerAccessor, true);
        }

        public IDomainRepository CreateDomainRepository()
        {
            return new NhDomainRepository(_sessionFactory.OpenSession(), _relayWorker.Value);
        }
    }
}

using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Transports;

namespace Taro.Persistence.NHibernate
{
    public class NhDomainRepository : DomainRepositoryBase
    {
        private ISession _session;

        public ISession Session
        {
            get { return _session; }
        }

        public NhDomainRepository(ISession session, IEventBus eventBus, IRelayWorker relayWorker)
            : base(eventBus, relayWorker)
        {
            _session = session;
        }

        public override T Find<T>(object id)
        {
            return _session.Get<T>(id);
        }

        protected override void SaveWithoutCommit<T>(T aggregate)
        {
            _session.Save(aggregate);
        }

        protected override void DeleteWithoutCommit<T>(T aggregate)
        {
            _session.Delete(aggregate);
        }

        protected override ILocalTransactionContext CreateLocalTransactionContext()
        {
            return new NhLocalTransactionContext(_session, _session.BeginTransaction(), false);
        }
    }
}

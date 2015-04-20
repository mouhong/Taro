using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence.NHibernate
{
    public class NhLocalTransactionContextFactory : ILocalTransactionContextFactory
    {
        private ISession _session;

        public NhLocalTransactionContextFactory(ISession session)
        {
            _session = session;
        }

        public ILocalTransactionContext CreateLocalTransactionContext()
        {
            return new NhLocalTransactionContext(_session, _session.BeginTransaction(), true);
        }
    }
}

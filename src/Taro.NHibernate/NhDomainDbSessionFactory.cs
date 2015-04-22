using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.NHibernate
{
    public class NhDomainDbSessionFactory : IDomainDbSessionFactory
    {
        private ISessionFactory _sessionFactory;

        public NhDomainDbSessionFactory(ISessionFactory sessionFactory)
        {
            Require.NotNull(sessionFactory, "sessionFactory");
            _sessionFactory = sessionFactory;
        }

        public IDomainDbSession OpenSession()
        {
            return new NhDomainDbSession(_sessionFactory.OpenSession());
        }
    }
}

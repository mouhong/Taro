using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Configuration;
using Taro.NHibernate;
using Taro.Persistence;
using Taro.Workers;

namespace Taro
{
    public static class NHibernateConfiguration
    {
        public static void UseNHibernate(this AppConfigurator configurator, ISessionFactory sessionFactory)
        {
            var runtime = configurator.AppRuntime;

            runtime.SetItem<IDomainDbSessionFactory>(new NhDomainDbSessionFactory(sessionFactory));
            runtime.SetItem<IDomainRepositoryFactory>(new NhDomainRepositoryFactory(sessionFactory, runtime.GetItem<IRelayWorker>));
        }
    }
}

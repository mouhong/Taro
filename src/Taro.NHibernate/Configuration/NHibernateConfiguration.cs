using NHibernate;
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

            runtime.Container.Register<IDomainDbSession>(_ => new NhDomainDbSession(sessionFactory.OpenSession()));
            runtime.Container.Register<IDomainRepository>(_ => new NhDomainRepository(_.Resolve<IDomainDbSession>(), _.Resolve<IRelayWorker>()));
        }
    }
}

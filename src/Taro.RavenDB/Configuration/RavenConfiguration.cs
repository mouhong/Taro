using Raven.Client;
using Taro.Configuration;
using Taro.Persistence;
using Taro.RavenDB;
using Taro.RavenDB.Indexes;
using Taro.Workers;

namespace Taro
{
    public static class RavenConfiguration
    {
        public static void UseRavenDB(this AppConfigurator configurator, IDocumentStore documentStore)
        {
            documentStore.ExecuteIndex(new StoredEventIndex());
            documentStore.Conventions.JsonContractResolver = new Taro.RavenDB.Serialization.AggregateRootContractResolver();

            var runtime = configurator.AppRuntime;
            runtime.Container.Register<IDomainDbSession>(_ => new RavenDomainDbSession(documentStore.OpenSession()));
            runtime.Container.Register<IDomainRepository>(_ => new RavenDomainRepository(_.Resolve<IDomainDbSession>(), _.Resolve<IRelayWorker>()));
        }
    }
}

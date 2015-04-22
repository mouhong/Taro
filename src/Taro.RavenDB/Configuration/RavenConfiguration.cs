using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            runtime.SetItem<IDomainDbSessionFactory>(new RavenDomainDbSessionFactory(documentStore));
            runtime.SetItem<IDomainRepositoryFactory>(new RavenDomainRepositoryFactory(documentStore, runtime.GetItem<IRelayWorker>));
        }
    }
}

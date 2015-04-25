using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Taro.Configuration;
using Taro.Persistence;
using Taro.Transports;
using Taro.Workers;

namespace Taro
{
    public class AppRuntime : HideObjectMembers
    {
        public static AppRuntime Instance = new AppRuntime();

        public ObjectContainer Container { get; private set; }

        public AppRuntime()
        {
            Container = new ObjectContainer();
        }

        public T CreateDomainRepository<T>() where T : IDomainRepository
        {
            return (T)Container.Resolve<IDomainRepository>();
        }

        public AppRuntime Configure(Action<AppConfigurator> configure)
        {
            configure(new AppConfigurator(this));
            return this;
        }

        public void Validate()
        {
            if (!Container.HasRegistrationFor<IRelayWorker>())
                throw new Exception("Rely worker is not registered.");

            if (!Container.HasRegistrationFor<IDomainRepository>())
                throw new Exception("Domain repository is not registered.");
        }

        public void Start()
        {
            Validate();
            Container.Resolve<IRelayWorker>().Start();
        }

        public Task Stop()
        {
            var relayWorker = Container.Resolve<IRelayWorker>();
            if (relayWorker != null)
            {
                return relayWorker.Stop();
            }

            return Task.FromResult(0);
        }
    }
}

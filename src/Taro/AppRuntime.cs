using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taro.Configuration;
using Taro.Persistence;
using Taro.Transports;
using Taro.Workers;

namespace Taro
{
    public class AppRuntime : IHideObjectMembers
    {
        public static AppRuntime Instance = new AppRuntime();

        private bool _configured;

        public IDictionary<string, object> Items { get; private set; }

        public AppRuntime()
        {
            Items = new DefaultValuedDictionary<string, object>();
        }

        public T CreateDomainRepository<T>() where T : IDomainRepository
        {
            return (T)this.GetItem<IDomainRepositoryFactory>().CreateDomainRepository();
        }

        public AppRuntime Configure(Action<AppConfigurator> configure)
        {
            configure(new AppConfigurator(this));
            _configured = true;
            return this;
        }

        public void Validate()
        {
            if (this.GetItem<IRelayWorker>() == null)
                throw new Exception("Rely worker is not configured.");

            if (this.GetItem<IDomainRepositoryFactory>() == null)
                throw new Exception("Domain repository factory is not configured.");
        }

        public void Start()
        {
            if (!_configured)
                throw new Exception("Configure before start the runtime.");

            Validate();
            
            this.GetItem<IRelayWorker>().Start();
        }

        public Task Stop()
        {
            if (!_configured)
            {
                return Task.FromResult<int>(0);
            }

            var relayWorker = this.GetItem<IRelayWorker>();
            if (relayWorker == null)
            {
                return Task.FromResult<int>(0);
            }

            return relayWorker.Stop();
        }
    }
}

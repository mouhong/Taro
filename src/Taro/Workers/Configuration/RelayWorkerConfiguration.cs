using System;
using Taro.Configuration;
using Taro.Persistence;
using Taro.Transports;
using Taro.Workers;
using Taro.Workers.Configuration;

namespace Taro
{
    public static class RelayWorkerConfiguration
    {
        public static void RunRelayWorkerInCurrentProcess(this AppConfigurator configurator)
        {
            RunRelayWorkerInCurrentProcess(configurator, cfg =>
            {
                cfg.UseInProcessEventTransport(x =>
                {
                    x.RegisterHandlers(AppDomain.CurrentDomain.GetAssemblies());
                });
            });
        }

        public static void RunRelayWorkerInCurrentProcess(this AppConfigurator configurator, Action<InProcessRelayWorkerConfigurator> configure)
        {
            var runtime = configurator.AppRuntime;

            var domainDbSessionFactory = runtime.GetItem<IDomainDbSessionFactory>();
            if (domainDbSessionFactory == null)
                throw new Exception("Please configure domain db session factory first.");

            var config = new InProcessRelayWorkerConfigurator(configurator.AppRuntime);
            configure(config);
            
            var relayWorker = new RelayWorker(domainDbSessionFactory, runtime.GetItem<IEventTransport>());
            runtime.SetItem<IRelayWorker>(relayWorker);
        }

        public static void UseRemoteRelayWorker(this AppConfigurator configurator, string serverUrl)
        {
            configurator.AppRuntime.SetItem<IRelayWorker>(new RemoteRelayWorker(serverUrl));
        }
    }
}

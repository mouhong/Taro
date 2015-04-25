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
            var config = new InProcessRelayWorkerConfigurator(configurator.AppRuntime);
            configure(config);

            var container = configurator.AppRuntime.Container;
            container.Register<IRelayWorker>(new RelayWorker(() => container.Resolve<IDomainDbSession>(), container.Resolve<IEventTransport>()));
        }

        public static void UseRemoteRelayWorker(this AppConfigurator configurator, string serverUrl)
        {
            configurator.AppRuntime.Container.Register<IRelayWorker>(new RemoteRelayWorker(serverUrl));
        }
    }
}

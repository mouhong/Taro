using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Taro.Transports;
using Taro.Transports.InProcess;
using Taro.Configuration;

namespace Taro.Workers.Configuration
{
    public class InProcessRelayWorkerConfigurator : HideObjectMembers
    {
        private AppRuntime _appRuntime;

        public InProcessRelayWorkerConfigurator(AppRuntime appRuntime)
        {
            _appRuntime = appRuntime;
        }

        public void UseInProcessEventTransport(Action<InProcessEventTransportConfigurator> configure)
        {
            var transport = new InProcessEventTransport();
            configure(new InProcessEventTransportConfigurator(transport));
            _appRuntime.Container.Register<IEventTransport>(transport);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Taro.Transports;
using Taro.Transports.InProcess;

namespace Taro.Workers.Configuration
{
    public class InProcessEventTransportConfigurator : IHideObjectMembers
    {
        private InProcessEventTransport _transport;

        public InProcessEventTransportConfigurator(InProcessEventTransport transport)
        {
            _transport = transport;
        }

        public void RegisterHandlers(IEnumerable<Assembly> assemblies)
        {
            if (assemblies != null)
                _transport.Registry.RegisterHandlers(assemblies);
        }

        public void RegisterHandlers(IEnumerable<Type> handlerTypes)
        {
            if (handlerTypes != null)
                _transport.Registry.RegisterHandlers(handlerTypes);
        }

        public void ActivateHandlersWith(IHandlerActivator activator)
        {
            if (activator != null)
            {
                _transport.HandlerActivator = activator;
            }
        }
    }
}

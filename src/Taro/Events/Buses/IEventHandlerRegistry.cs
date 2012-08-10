using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Taro.Events.Buses
{
    public interface IEventHandlerRegistry
    {
        IEnumerable FindHandlers(Type eventType);

        bool RegisterHandler(Type handlerType);

        void RegisterHandlers(IEnumerable<Assembly> assembliesToScan);

        bool UnregisterHandler(Type handlerType);

        void UnregisterHandlers(Type eventType);

        void UnregisterAllHandlers();
    }
}

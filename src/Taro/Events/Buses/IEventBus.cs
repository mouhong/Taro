using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Taro.Events.Buses
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent evnt) where TEvent : IEvent;

        bool RegisterHandler(Type handlerType);

        void RegisterHandlers(IEnumerable<Assembly> assembliesToScan);

        bool UnregisterHandler(Type handlerType);

        void UnregisterHandlers(Type eventType);

        void UnregisterAllHandlers();
    }
}

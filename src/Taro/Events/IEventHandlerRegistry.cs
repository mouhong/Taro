using System;
using System.Collections.Generic;
using System.Reflection;

namespace Taro.Events
{
    public interface IEventHandlerRegistry
    {
        IEnumerable<Type> FindHandlerTypes(Type eventType);

        bool RegisterHandler(Type handlerType);

        void RegisterHandlers(Assembly assembly);
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Taro.Dispatching
{
    public interface IEventHandlerRegistry
    {
        IEnumerable<MethodInfo> FindHandlerMethods(Type eventType);

        void RegisterHandlers(IEnumerable<Type> handlerTypes);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);

        void Clear();
    }
}

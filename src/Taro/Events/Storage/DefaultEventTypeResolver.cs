using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Storage
{
    public class DefaultEventTypeResolver : IEventTypeResolver
    {
        public Type ResolveType(string eventTypeName)
        {
            return Type.GetType(eventTypeName, true);
        }

        public string GetTypeName(Type eventType)
        {
            return eventType.AssemblyQualifiedName;
        }
    }
}

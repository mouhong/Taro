using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Storage
{
    public interface IEventTypeResolver
    {
        Type ResolveType(string eventTypeName);

        string GetTypeName(Type eventType);
    }

    public static class EventTypeResolvers
    {
        public static Func<IEventTypeResolver> Current = () => new DefaultEventTypeResolver();
    }
}

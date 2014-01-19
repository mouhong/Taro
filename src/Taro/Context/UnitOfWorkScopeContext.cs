using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Taro.Context
{
    public static class UnitOfWorkScopeContext
    {
        public static EventContext Current
        {
            get
            {
                return ThreadLocalContextStack<EventContext>.Current;
            }
        }

        public static void Bind(EventContext scope)
        {
            Require.NotNull(scope, "scope");
            ThreadLocalContextStack<EventContext>.Bind(scope);
        }

        public static void Unbind()
        {
            ThreadLocalContextStack<EventContext>.Unbind();
        }
    }
}

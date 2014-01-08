using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Taro.Context
{
    public static class UnitOfWorkScopeContext
    {
        public static UnitOfWorkScope Current
        {
            get
            {
                return ThreadLocalContextStack<UnitOfWorkScope>.Current;
            }
        }

        public static void Bind(UnitOfWorkScope scope)
        {
            Require.NotNull(scope, "scope");
            ThreadLocalContextStack<UnitOfWorkScope>.Bind(scope);
        }

        public static void Unbind()
        {
            ThreadLocalContextStack<UnitOfWorkScope>.Unbind();
        }
    }
}

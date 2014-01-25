using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Taro.Dispatching
{
    static class UnitOfWorkScopeContext
    {
        static readonly ThreadLocal<Stack<UnitOfWorkScope>> _scopes = new ThreadLocal<Stack<UnitOfWorkScope>>(() => new Stack<UnitOfWorkScope>());

        public static UnitOfWorkScope Current
        {
            get
            {
                var stack = _scopes.Value;
                return stack.Count == 0 ? null : stack.Peek();
            }
        }

        public static void Bind(UnitOfWorkScope scope)
        {
            Require.NotNull(scope, "scope");
            _scopes.Value.Push(scope);
        }

        public static void Unbind()
        {
            var stack = _scopes.Value;
            if (stack.Count == 0)
                throw new InvalidOperationException("No UnitOfWorkScope was binded.");

            stack.Pop();
        }
    }
}

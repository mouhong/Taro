using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Taro.Context
{
    static class ThreadLocalContextStack<TContext>
        where TContext : class
    {
        static readonly ThreadLocal<Stack<TContext>> _scopes = new ThreadLocal<Stack<TContext>>(() => new Stack<TContext>());

        public static TContext Current
        {
            get
            {
                var stack = _scopes.Value;
                return stack.Count == 0 ? null : stack.Peek();
            }
        }

        public static void Bind(TContext context)
        {
            Require.NotNull(context, "context");
            _scopes.Value.Push(context);
        }

        public static void Unbind()
        {
            var stack = _scopes.Value;
            if (stack.Count == 0)
                throw new InvalidOperationException("No context was binded.");

            stack.Pop();
        }
    }
}

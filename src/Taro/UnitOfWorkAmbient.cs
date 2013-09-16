using System;
using System.Collections.Generic;
using System.Threading;

namespace Taro
{
    public static class UnitOfWorkAmbient
    {
        [ThreadStatic]
        private static IUnitOfWork _current;

        static readonly ThreadLocal<Stack<IUnitOfWork>> _unitOfWorks = new ThreadLocal<Stack<IUnitOfWork>>(() => new Stack<IUnitOfWork>());

        public static IUnitOfWork Current
        {
            get
            {
                var stack = _unitOfWorks.Value;
                return stack.Count == 0 ? null : stack.Peek();
            }
        }

        public static void Bind(IUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            _unitOfWorks.Value.Push(unitOfWork);
        }

        public static void Unbind()
        {
            var stack = _unitOfWorks.Value;
            if (stack.Count == 0)
                throw new InvalidOperationException("No unit of work was binded.");

            stack.Pop();
        }
    }
}

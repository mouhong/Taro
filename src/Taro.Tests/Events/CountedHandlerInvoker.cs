using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taro.Events;

namespace Taro.Tests.Events
{
    public class CountedHandlerInvoker : IHandlerInvoker
    {
        private IHandlerInvoker _innerInvoker = new DefaultHandlerInvoker();
        // Key: handler type, value: times invoked
        private Dictionary<Type, int> _invokations = new Dictionary<Type, int>();

        public void Invoke(object handler, MethodInfo handlerMethod, IDomainEvent evnt, EventDispatchingContext context)
        {
            if (!_invokations.ContainsKey(handlerMethod.DeclaringType))
            {
                _invokations.Add(handlerMethod.DeclaringType, 0);
            }

            _invokations[handlerMethod.DeclaringType]++;

            _innerInvoker.Invoke(handler, handlerMethod, evnt, context);
        }

        public void ClearCounter()
        {
            _invokations.Clear();
        }

        public int GetTotalInvoked(Type handlerType)
        {
            if (_invokations.ContainsKey(handlerType))
            {
                return _invokations[handlerType];
            }

            return 0;
        }
    }
}

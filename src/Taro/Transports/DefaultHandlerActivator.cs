using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Taro.Transports
{
    public class DefaultHandlerActivator : IHandlerActivator
    {
        private readonly ConcurrentDictionary<Type, Func<object>> _activators = new ConcurrentDictionary<Type, Func<object>>();

        public object CreateInstance(Type handlerType)
        {
            var activate = _activators.GetOrAdd(handlerType, type =>
            {
                return Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
            });

            return activate();
        }
    }
}

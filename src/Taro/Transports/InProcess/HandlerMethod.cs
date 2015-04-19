using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Taro.Transports.InProcess
{
    public class HandlerMethod
    {
        private MethodInfo _methodInfo;
        private Action<object, object> _invoke;

        public string Name
        {
            get { return _methodInfo.Name; }
        }

        public Type ReflectedType
        {
            get { return _methodInfo.ReflectedType; }
        }

        public HandlerMethod(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            CompileInvoker();
        }

        private void CompileInvoker()
        {
            var instance = Expression.Parameter(typeof(object));
            var evnt = Expression.Parameter(typeof(object));
            _invoke = Expression.Lambda<Action<object, object>>(Expression.Call(instance, _methodInfo), evnt).Compile();
        }

        public void Invoke(object handlerInstance, IEvent @event)
        {
            _invoke(handlerInstance, @event);
        }
    }
}

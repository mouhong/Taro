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
            var theEvent = Expression.Parameter(typeof(object));

            var handlerType = _methodInfo.ReflectedType;
            var eventType = _methodInfo.GetParameters()[0].ParameterType;

            _invoke = Expression.Lambda<Action<object, object>>(
                Expression.Call(Expression.TypeAs(instance, handlerType), _methodInfo, Expression.TypeAs(theEvent, eventType)), instance, theEvent).Compile();
        }

        public void Invoke(object handlerInstance, IEvent theEvent)
        {
            _invoke(handlerInstance, theEvent);
        }
    }
}

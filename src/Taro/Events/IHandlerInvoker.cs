using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Taro.Events
{
    public interface IHandlerInvoker
    {
        void Invoke(IDomainEvent evnt, MethodInfo handlerMethod, EventDispatchingContext context);
    }
}

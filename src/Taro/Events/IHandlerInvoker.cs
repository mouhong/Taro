using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Taro.Events
{
    public interface IHandlerInvoker
    {
        void Invoke(object handler, MethodInfo handleMethod, IEvent evnt, EventDispatchingContext context);
    }

    public class DefaultHandlerInvoker : IHandlerInvoker
    {
        public void Invoke(object handler, MethodInfo handleMethod, IEvent evnt, EventDispatchingContext context)
        {
            handleMethod.Invoke(handler, new object[] { evnt });
        }
    }
}

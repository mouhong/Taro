using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Dispatching
{
    public interface IHandlerActivator
    {
        object CreateHandlerInstance(Type handlerType, EventDispatchingContext dispatchingContext);
    }

    public class DefaultHandlerActivator : IHandlerActivator
    {
        public object CreateHandlerInstance(Type handlerType, EventDispatchingContext dispatchingContext)
        {
            try
            {
                var handler = Activator.CreateInstance(handlerType);
                OnHandlerInstanceCreated(handler, dispatchingContext);
                return handler;
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Failed creating event handler instance. Handler type: " + handlerType + ".", ex);
            }
        }

        protected virtual void OnHandlerInstanceCreated(object handler, EventDispatchingContext dispatchingContext) { }
    }
}

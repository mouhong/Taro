using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events
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
                var unitOfWorkAware = handler as IUnitOfWorkAware;

                if (unitOfWorkAware != null && dispatchingContext.UnitOfWorkScope != null)
                {
                    unitOfWorkAware.UnitOfWork = dispatchingContext.UnitOfWorkScope.UnitOfWork;
                }

                return handler;
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Failed creating event handler instance. Handler type: " + handlerType + ".", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Events
{
    public class DefaultHandlerInvoker : IHandlerInvoker
    {
        public void Invoke(IDomainEvent evnt, MethodInfo handlerMethod, EventDispatchingContext context)
        {
            if (TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(handlerMethod, typeof(HandleAsyncAttribute)))
            {
                Task.Factory.StartNew(() => InvokeHandler(evnt, handlerMethod, context));
            }
            else
            {
                InvokeHandler(evnt, handlerMethod, context);
            }
        }

        private void InvokeHandler(IDomainEvent evnt, MethodInfo method, EventDispatchingContext context)
        {
            var handlerType = method.DeclaringType;
            var handler = CreateHandlerInstance(handlerType, context);

            try
            {
                method.Invoke(handler, new object[] { evnt });
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Event handler throws an exception, please check inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }

        private object CreateHandlerInstance(Type handlerType, EventDispatchingContext context)
        {
            try
            {
                var handler = Activator.CreateInstance(handlerType);
                var unitOfWorkAware = handler as IUnitOfWorkAware;

                if (unitOfWorkAware != null && context.UnitOfWorkScope != null)
                {
                    unitOfWorkAware.UnitOfWork = context.UnitOfWorkScope.UnitOfWork;
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

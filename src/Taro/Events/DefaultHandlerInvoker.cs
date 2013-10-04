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
                var unitOfWorkType = TypeUtil.GetOpenGenericArgumentTypes(handlerType, typeof(IUnitOfWorkAware<>)).FirstOrDefault();

                if (unitOfWorkType != null)
                {
                    if (!unitOfWorkType.IsAssignableFrom(context.UnitOfWork.GetType()))
                        throw new EventHandlerException("Handler requires to be aware of unit of work of type \"" + unitOfWorkType + "\", but the unit of work in current context is of type \"" + context.UnitOfWork.GetType() + "\".");

                    var prop = handlerType.GetProperty("UnitOfWork", BindingFlags.Public | BindingFlags.Instance | BindingFlags.ExactBinding);

                    if (!prop.CanWrite)
                        throw new EventHandlerException("IUnitOfWorkAware.UnitOfWork property must be writable.");

                    prop.SetValue(handler, context.UnitOfWork, null);
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

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Taro.Events
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IEventHandlerRegistry _handlerRegistry;

        public IEventHandlerRegistry HandlerRegistry
        {
            get
            {
                return _handlerRegistry;
            }
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");
            _handlerRegistry = handlerRegistry;
        }

        public void Dispatch(IDomainEvent evnt, EventDispathcingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            foreach (var method in _handlerRegistry.FindHandlerMethods(evnt.GetType()))
            {
                if (!context.WasUnitOfWorkCommitted && HandlerUtil.IsAttributeDefined(method, typeof(AwaitCommittedAttribute)))
                {
                    continue;
                }

                if (HandlerUtil.IsAttributeDefined(method, typeof(HandleAsyncAttribute)))
                {
                    Task.Factory.StartNew(() => InvokeHandler(evnt, method, context));
                }
                else
                {
                    InvokeHandler(evnt, method, context);
                }
            }
        }

        private void InvokeHandler(IDomainEvent evnt, MethodInfo method, EventDispathcingContext context)
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

        private object CreateHandlerInstance(Type handlerType, EventDispathcingContext context)
        {
            try
            {
                var handler = Activator.CreateInstance(handlerType);

                if (handler is IUnitOfWorkAware)
                {
                    var unitOfWorkAware = (IUnitOfWorkAware)handler;
                    unitOfWorkAware.UnitOfWork = context.UnitOfWork;
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

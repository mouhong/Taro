using System;
using System.Threading.Tasks;

namespace Taro.Events
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IEventHandlerRegistry _handlerRegistry;

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");
            _handlerRegistry = handlerRegistry;
        }

        public void Dispatch(IDomainEvent evnt, EventDispathcingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            foreach (var handlerType in _handlerRegistry.FindHandlerTypes(evnt.GetType()))
            {
                if (!NeedToHandle(handlerType, context))
                {
                    continue;
                }

                if (handlerType.IsDefined(typeof(HandleAsyncAttribute), false))
                {
                    Task.Factory.StartNew(() => InvokeHandler(evnt, handlerType, context));
                }
                else
                {
                    InvokeHandler(evnt, handlerType, context);
                }
            }
        }

        private void InvokeHandler(IDomainEvent evnt, Type handlerType, EventDispathcingContext context)
        {
            var handler = CreateHandlerInstance(handlerType, context);
            var method = TypeUtil.FindHandleMethod(handlerType, evnt.GetType());

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
                throw new EventHandlerException("Cannot create handler instance. Handler type: " + handlerType + ".", ex);
            }
        }

        private bool NeedToHandle(Type handlerType, EventDispathcingContext context)
        {
            var isPostCommitEventHandler = handlerType.IsDefined(typeof(AwaitCommittedAttribute), false);

            if (context.WasUnitOfWorkCommitted)
            {
                return isPostCommitEventHandler;
            }

            return !isPostCommitEventHandler;
        }
    }
}

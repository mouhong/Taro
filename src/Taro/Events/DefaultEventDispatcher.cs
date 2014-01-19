using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Taro.Events
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IEventHandlerRegistry _handlerRegistry;
        private IHandlerActivator _handlerActivator = new DefaultHandlerActivator();
        private IHandlerInvoker _handlerInvoker = new DefaultHandlerInvoker();

        public IEventHandlerRegistry HandlerRegistry
        {
            get
            {
                return _handlerRegistry;
            }
        }

        public IHandlerActivator HandlerActivator
        {
            get
            {
                return _handlerActivator;
            }
            set
            {
                _handlerActivator = value;
            }
        }

        public IHandlerInvoker HandlerInvoker
        {
            get
            {
                return _handlerInvoker;
            }
            set
            {
                _handlerInvoker = value;
            }
        }

        public DefaultEventDispatcher()
            : this(new DefaultEventHandlerRegistry())
        {
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");

            _handlerRegistry = handlerRegistry;
        }

        public void Dispatch(IEvent evnt, EventDispatchingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            foreach (var method in _handlerRegistry.FindHandlerMethods(evnt.GetType()))
            {
                // if it's not within a unit of work scope, then simply invoke all handlers
                if (context.UnitOfWorkScope == null)
                {
                    ExecuteHandler(method, evnt, context);
                }
                else
                {
                    var needToAwaitCommit = TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute));
                    if (!needToAwaitCommit && context.Phase == EventDispatchingPhase.OnEventRaised)
                    {
                        ExecuteHandler(method, evnt, context);
                    }
                    if (needToAwaitCommit && context.Phase == EventDispatchingPhase.OnUnitOfWorkCommitted)
                    {
                        ExecuteHandler(method, evnt, context);
                    }
                }
            }
        }

        private void ExecuteHandler(MethodInfo handlerMethod, IEvent evnt, EventDispatchingContext context)
        {
            if (TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(handlerMethod, typeof(HandleAsyncAttribute)))
            {
                Task.Factory.StartNew(() => DoExecuteHandler(handlerMethod, evnt, context));
            }
            else
            {
                DoExecuteHandler(handlerMethod, evnt, context);
            }
        }

        private void DoExecuteHandler(MethodInfo handlerMethod, IEvent evnt, EventDispatchingContext context)
        {
            var handlerType = handlerMethod.DeclaringType;
            object handler = null;

            try
            {
                handler = _handlerActivator.CreateHandlerInstance(handlerType, context);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Faile to create handler instance, see inner exception for detail.", ex);
            }

            try
            {
                _handlerInvoker.Invoke(handler, handlerMethod, evnt, context);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Faile to execute event handler, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }
    }
}

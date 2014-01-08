using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Taro.Events
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IEventHandlerRegistry _handlerRegistry;
        private IHandlerInvoker _handlerInvoker;

        public IEventHandlerRegistry HandlerRegistry
        {
            get
            {
                return _handlerRegistry;
            }
        }

        public IHandlerInvoker HandlerInvoker
        {
            get
            {
                return _handlerInvoker;
            }
        }

        public DefaultEventDispatcher()
            : this(new DefaultEventHandlerRegistry())
        {
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
            : this(handlerRegistry, new DefaultHandlerInvoker())
        {
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry, IHandlerInvoker handlerInvoker)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");
            Require.NotNull(handlerInvoker, "handlerInvoker");

            _handlerRegistry = handlerRegistry;
            _handlerInvoker = handlerInvoker;
        }

        public void Dispatch(IDomainEvent evnt, EventDispatchingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            foreach (var method in _handlerRegistry.FindHandlerMethods(evnt.GetType()))
            {
                // if it's not within a unit of work scope, then simply invoke all handlers
                if (context.UnitOfWorkScope == null)
                {
                    _handlerInvoker.Invoke(evnt, method, context);
                }
                else
                {
                    var needToAwaitCommit = TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute));
                    if (!needToAwaitCommit && context.Phase == EventDispatchingPhase.OnEventRaised)
                    {
                        _handlerInvoker.Invoke(evnt, method, context);
                    }
                    if (needToAwaitCommit && context.Phase == EventDispatchingPhase.OnUnitOfWorkCommitted)
                    {
                        _handlerInvoker.Invoke(evnt, method, context);
                    }
                }
            }
        }
    }
}

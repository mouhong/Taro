using System;
using System.Collections.Generic;
using System.Reflection;
using Taro.Events;

namespace Taro.Config
{
    public class TaroEnvironment
    {
        public static readonly TaroEnvironment Instance = new TaroEnvironment();

        public IEventDispatcher EventDispatcher { get; private set; }

        public Func<IUnitOfWork> UnitOfWorkFactory { get; private set; }

        private TaroEnvironment()
        {
        }

        public static void Configure(Action<TaroEnvironment> action)
        {
            Require.NotNull(action, "action");
            action(Instance);
        }

        public TaroEnvironment UsingUnitOfWorkFactory(Func<IUnitOfWork> unitOfWorkFactory)
        {
            Require.NotNull(unitOfWorkFactory, "unitOfWorkFactory");
            UnitOfWorkFactory = unitOfWorkFactory;
            return this;
        }

        public TaroEnvironment UsingDefaultEventDispatcher(params Assembly[] handlerAssemblies)
        {
            return UsingDefaultEventDispatcher(handlerAssemblies as IEnumerable<Assembly>);
        }

        public TaroEnvironment UsingDefaultEventDispatcher(IEnumerable<Assembly> handlerAssemblies)
        {
            Require.NotNull(handlerAssemblies, "handlerAssemblies");

            var registry = new DefaultEventHandlerRegistry();

            foreach (var asm in handlerAssemblies)
            {
                registry.RegisterHandlers(asm);
            }

            EventDispatcher = new DefaultEventDispatcher(registry);

            return this;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            if (UnitOfWorkFactory == null)
                throw new InvalidOperationException("Please register unit of work factory first.");

            var unitOfWork = UnitOfWorkFactory();

            if (unitOfWork == null)
                throw new InvalidOperationException("Unit of work factory returns null.");

            return unitOfWork;
        }
    }
}

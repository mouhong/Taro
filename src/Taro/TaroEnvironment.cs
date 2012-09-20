using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Taro.Data;
using Taro.Events;
using Taro.Events.Buses;
using Taro.Utils;

namespace Taro
{
    public class TaroEnvironment
    {
        public static readonly TaroEnvironment Instance = new TaroEnvironment();

        public IEventBus ImmediateEventBus { get; set; }

        public IEventBus PostCommitEventBus { get; set; }

        public Func<IUnitOfWork> UnitOfWorkFactory { get; set; }

        private TaroEnvironment()
        {
            ImmediateEventBus = new DefaultEventBus(new ImmediateEventHandlerRegistry());
            PostCommitEventBus = new DefaultEventBus(new PostCommitEventHandlerRegistry());
        }

        public static void Configure(Action<TaroEnvironment> action)
        {
            Require.NotNull(action, "action");
            action(Instance);
        }

        public TaroEnvironment RegisterEventHandlers(params Assembly[] assembliesToScan)
        {
            return RegisterEventHandlers(assembliesToScan as IEnumerable<Assembly>);
        }

        public TaroEnvironment RegisterEventHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            Require.NotNull(assembliesToScan, "assembliesToScan");

            var immediateEventBus = ImmediateEventBus;
            var postCommitEventBus = PostCommitEventBus;

            if (immediateEventBus == null)
                throw new InvalidOperationException("Please register immediate event bus to the TaroEnvironment first.");

            if (postCommitEventBus == null)
                throw new InvalidOperationException("Please register post commit event bus to the TaroEvnironment first.");

            immediateEventBus.RegisterHandlers(assembliesToScan);
            postCommitEventBus.RegisterHandlers(assembliesToScan);

            return this;
        }

        public TaroEnvironment RegisterUnitOfWorkFactory(Func<IUnitOfWork> factory)
        {
            Require.NotNull(factory, "factory");
            UnitOfWorkFactory = factory;
            return this;
        }
    }
}

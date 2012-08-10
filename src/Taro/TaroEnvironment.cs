using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Taro.Data;
using Taro.Events;
using Taro.Events.Buses;
using Taro.Events.Storage;
using Taro.Events.Storage.Rdbms;
using Taro.Utils;

namespace Taro
{
    public class TaroEnvironment
    {
        public static readonly TaroEnvironment Instance = new TaroEnvironment();

        public IEventBus ImmediateEventBus { get; set; }

        public IEventBus PostCommitEventBus { get; set; }

        public IEventStore EventStore { get; set; }

        public Func<IUnitOfWork> UnitOfWorkFactory { get; set; }

        private TaroEnvironment()
        {
        }

        public static void Configure(Action<TaroEnvironment> action)
        {
            Require.NotNull(action, "action");
            action(Instance);
        }

        public TaroEnvironment UseDefaultEventBuses()
        {
            ImmediateEventBus = new DefaultEventBus(new ImmediateEventHandlerRegistry());
            PostCommitEventBus = new DefaultEventBus(new PostCommitEventHandlerRegistry());
            return this;
        }

        public TaroEnvironment RegisterEventHandlers(params Assembly[] assembliesToScan)
        {
            if (ImmediateEventBus == null)
            {
                ImmediateEventBus = new DefaultEventBus(new ImmediateEventHandlerRegistry());
            }
            if (PostCommitEventBus == null)
            {
                PostCommitEventBus = new DefaultEventBus(new PostCommitEventHandlerRegistry());
            }

            ImmediateEventBus.RegisterHandlers(assembliesToScan);
            PostCommitEventBus.RegisterHandlers(assembliesToScan);

            return this;
        }

        public TaroEnvironment RegisterUnitOfWorkFactory(Func<IUnitOfWork> factory)
        {
            Require.NotNull(factory, "factory");
            UnitOfWorkFactory = factory;
            return this;
        }

        public TaroEnvironment UseRdbmsEventStore(string connectionString, string dbProviderInvariantName, ISqlStatementProvider dialect)
        {
            EventStore = new RdbmsEventStore(connectionString, dbProviderInvariantName, dialect);
            return this;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Taro.Data;
using Taro.Events;
using Taro.Events.Buses;
using Taro.Commands;
using Taro.Commands.Buses;
using Taro.Events.Storage;
using Taro.Events.Storage.Rdbms;
using Taro.Utils;

namespace Taro
{
    public class TaroEnvironment
    {
        public static readonly TaroEnvironment Instance = new TaroEnvironment();

        public ImmediateHandlerRegistry ImmediateHandlerRegistry { get; private set; }

        public IEventBus EventBus { get; set; }

        public IEventStore EventStore { get; set; }

        public ICommandBus CommandBus { get; set; }

        private TaroEnvironment()
        {
            ImmediateHandlerRegistry = new ImmediateHandlerRegistry();
        }

        public TaroEnvironment RegisterImmediateHandlers(params Assembly[] assembliesToScan)
        {
            return RegisterImmediateHandlers(assembliesToScan as IEnumerable<Assembly>);
        }

        public TaroEnvironment RegisterImmediateHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            Require.NotNull(assembliesToScan, "assembliesToScan");

            ImmediateHandlerRegistry.RegisterHandlers(assembliesToScan);
            return this;
        }

        public TaroEnvironment UseDefaultEventBus(params Assembly[] handlerAssemblies)
        {
            return UseDefaultEventBus(handlerAssemblies as IEnumerable<Assembly>);
        }

        public TaroEnvironment UseDefaultEventBus(IEnumerable<Assembly> handlerAssemblies)
        {
            Require.NotNull(handlerAssemblies, "handlerAssemblies");

            var handlerFinder = new DefaultOnCommitEventHandlerFinder();
            handlerFinder.RegisterHandlers(handlerAssemblies);

            var bus = new DefaultEventBus(handlerFinder);

            EventBus = bus;

            return this;
        }

        public TaroEnvironment UseDefaultCommandBus(params Assembly[] executorAssemblies)
        {
            return UseDefaultCommandBus(executorAssemblies as IEnumerable<Assembly>);
        }

        public TaroEnvironment UseDefaultCommandBus(IEnumerable<Assembly> executorAssemblies)
        {
            Require.NotNull(executorAssemblies, "executorAssemblies");

            var executorFinder = new DefaultCommandExecutorFinder();
            executorFinder.RegisterExecutors(executorAssemblies);

            CommandBus = new DefaultCommandBus(executorFinder);

            return this;
        }

        public TaroEnvironment UseUnitOfWorkFactory(Func<IUnitOfWork> factory)
        {
            Require.NotNull(factory, "factory");
            UnitOfWorkFactory.Get = factory;
            return this;
        }

        public TaroEnvironment UseRdbmsEventStore(string connectionString, string dbProviderInvariantName, ISqlStatementProvider dialect)
        {
            EventStore = new RdbmsEventStore(connectionString, dbProviderInvariantName, dialect);
            return this;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Taro.Events;

namespace Taro.Config
{
    public class TaroEnvironment
    {
        public static readonly TaroEnvironment Instance = new TaroEnvironment();

        public IEventDispatcher EventDispatcher { get; private set; }

        private TaroEnvironment()
        {
        }

        public static void Configure(Action<TaroEnvironment> action)
        {
            Require.NotNull(action, "action");
            action(Instance);
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
    }
}

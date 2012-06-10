using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events.Storage.RavenDB;
using Taro.Events.Storage;
using Taro.Utils;

namespace Taro
{
    public static class RavenEventStoreTaroEnvironmentExtensions
    {
        public static TaroEnvironment UseRavenEventStore(this TaroEnvironment environment, string connectionStringName)
        {
            RavenDatabase.Initialize(connectionStringName);
            environment.EventStore = new RavenEventStore(RavenDatabase.DocumentStore);

            return environment;
        }

        public static TaroEnvironment UseRavenEventStore(this TaroEnvironment environment, RavenEventStore store)
        {
            Require.NotNull(environment, "registry");
            Require.NotNull(store, "store");

            environment.EventStore = store;

            return environment;
        }
    }
}

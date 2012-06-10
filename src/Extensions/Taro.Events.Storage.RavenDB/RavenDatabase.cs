using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Taro.Utils;

namespace Taro.Events.Storage.RavenDB
{
    static class RavenDatabase
    {
        public static IDocumentStore DocumentStore { get; private set; }

        public static void Initialize(string connectionStringName)
        {
            Require.That(!String.IsNullOrWhiteSpace(connectionStringName), "'connectionStringName' is required.");

            Initialize(store =>
            {
                store.ConnectionStringName = connectionStringName;
            });
        }

        public static void Initialize(Action<DocumentStore> configure)
        {
            Require.NotNull(configure, "configure");

            var store = new DocumentStore();
            configure(store);

            store.Initialize();

            IndexCreation.CreateIndexes(typeof(RavenDatabase).Assembly, store);

            DocumentStore = store;
        }

        public static void Initialize(IDocumentStore store)
        {
            Require.NotNull(store, "store");
            store.Initialize();

            DocumentStore = store;
        }

        public static IDocumentSession OpenSession()
        {
            return DocumentStore.OpenSession();
        }
    }
}

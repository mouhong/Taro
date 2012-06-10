using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Taro.Serialization;
using Taro.Utils;

namespace Taro.Events.Storage.Rdbms
{
    public class RdbmsEventStore : IEventStore
    {
        protected ISqlStatementProvider SqlStatementProvider { get; private set; }

        protected ISerializer Serializer { get; private set; }

        protected IEventTypeResolver EventTypeResolver { get; private set; }

        protected string ConnectionString { get; private set; }

        protected DbProviderFactory DbProviderFactory { get; private set; }

        protected IsolationLevel IsolationLevel { get; private set; }

        protected IEventIdGenerator EventIdGenerator { get; private set; }

        public RdbmsEventStore(
            string connectionString,
            string dbProviderInvariantName,
            ISqlStatementProvider statementProvider)
            : this(connectionString, dbProviderInvariantName, statementProvider, null)
        {
        }

        public RdbmsEventStore(
            string connectionString,
            string dbProviderInvariantName,
            ISqlStatementProvider statementProvider,
            IEventIdGenerator eventIdGenerator)
            : this(connectionString, dbProviderInvariantName, statementProvider, Serializers.Current(), EventTypeResolvers.Current(), eventIdGenerator)
        {
        }

        public RdbmsEventStore(
            string connectionString,
            string dbProviderInvariantName,
            ISqlStatementProvider statementProvider,
            ISerializer serializer,
            IEventTypeResolver eventTypeResolver,
            IEventIdGenerator eventIdGenerator)
        {
            Require.That(!String.IsNullOrEmpty(connectionString), "'connectionString' is required.");
            Require.That(!String.IsNullOrEmpty(dbProviderInvariantName), "'dbProviderInvariantName' is required.");
            Require.NotNull(statementProvider, "statementProvider");
            Require.NotNull(serializer, "serializer");
            Require.NotNull(eventTypeResolver, "eventTypeResolver");

            SqlStatementProvider = statementProvider;
            Serializer = serializer;
            EventTypeResolver = eventTypeResolver;
            DbProviderFactory = DbProviderFactories.GetFactory(dbProviderInvariantName);
            ConnectionString = connectionString;
            IsolationLevel = System.Data.IsolationLevel.Serializable;
            EventIdGenerator = eventIdGenerator;
        }

        public virtual IEnumerable<IEvent> LoadEvents(int fromId, int toId)
        {
            using (var conn = OpenConnection())
            using (var cmd = conn.CreateCommand())
            {
                var statement = SqlStatementProvider.GetSelectStatement(() => cmd.CreateParameter(), fromId, toId);

                SetSqlStatement(cmd, statement);

                using (var reader = cmd.ExecuteReader())
                {
                    yield return ReadEvent(reader);
                }
            }
        }

        public virtual IEnumerable<IEvent> LoadEvents(DateTime? fromUtcTimestamp, DateTime? toUtcTimestamp)
        {
            using (var conn = OpenConnection())
            using (var cmd = conn.CreateCommand())
            {
                var statement = SqlStatementProvider.GetSelectStatement(() => cmd.CreateParameter(), fromUtcTimestamp, toUtcTimestamp);

                SetSqlStatement(cmd, statement);

                using (var reader = cmd.ExecuteReader())
                {
                    yield return ReadEvent(reader);
                }
            }
        }

        public virtual void SaveEvents(IEnumerable<IEvent> events)
        {
            using (var conn = OpenConnection())
            using (var tran = conn.BeginTransaction(IsolationLevel))
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tran;

                foreach (var evnt in events)
                {
                    cmd.Parameters.Clear();

                    var statement = SqlStatementProvider.GetInsertStatement(
                        () => cmd.CreateParameter(), EventIdGenerator == null ? null : (int?)EventIdGenerator.Generate(), evnt.UtcTimestamp, EventTypeResolver.GetTypeName(evnt.GetType()), Serializer.Serialize(evnt));

                    SetSqlStatement(cmd, statement);

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
        }

        protected virtual IEvent ReadEvent(IDataReader reader)
        {
            var data = reader["SerializedEventData"] as String;
            var eventTypeName = reader["EventTypeName"] as String;

            var eventType = EventTypeResolver.ResolveType(eventTypeName);

            return (IEvent)Serializer.Deserialize(data, eventType);
        }

        protected virtual IDbConnection OpenConnection()
        {
            var conn = DbProviderFactory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            conn.Open();

            return conn;
        }

        private void SetSqlStatement(IDbCommand command, SqlStatement statement)
        {
            command.CommandText = statement.Sql;
            command.Parameters.Clear();

            foreach (var param in statement.Parameters)
            {
                command.Parameters.Add(param);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Taro.Events.Storage.Rdbms
{
    public class SqlServerSqlStatementProvider : ISqlStatementProvider
    {
        public SqlStatement GetSelectStatement(
            Func<IDbDataParameter> createDbDataParameter, int fromId, int toId)
        {
            var sql = "SELECT * FROM " + Quote("Events") + " WHERE " + Quote("Id") + " > " + fromId + " AND " + Quote("Id") + " < " + toId;
            return new SqlStatement(sql);
        }
        
        public virtual SqlStatement GetSelectStatement(
            Func<IDbDataParameter> createDbDataParameter, DateTime? utcStartTimestamp, DateTime? utcEndTimeStamp)
        {
            var sql = String.Empty;
            var parameters = new List<IDbDataParameter>();

            if (utcStartTimestamp == null && utcEndTimeStamp == null)
            {
                sql = String.Format("SELECT * FROM {0} ORDER BY {1} DESC", Quote("Events"), Quote("UtcTimestamp"));
            }
            else
            {
                sql = "SELECT * FROM " + Quote("Events") + " WHERE ";

                if (utcStartTimestamp != null)
                {
                    sql += "UtcTimestamp >= " + GetDbParameterFullName("UtcStartTimestamp");

                    parameters.Add(CreateParameter(createDbDataParameter, "UtcStartTimestamp", DbType.DateTime, utcStartTimestamp.Value));
                }
                if (utcEndTimeStamp != null)
                {
                    if (utcStartTimestamp != null)
                    {
                        sql += " AND ";
                    }
                    sql += "UtcTimestamp < " + GetDbParameterFullName("UtcEndTimestamp");

                    parameters.Add(CreateParameter(createDbDataParameter, "UtcEndTimestamp", DbType.DateTime, utcEndTimeStamp.Value));
                }
            }

            return new SqlStatement(sql, parameters);
        }

        public virtual SqlStatement GetInsertStatement(
            Func<IDbDataParameter> createDbDataParameter, int? eventId, DateTime utcTimestamp, string eventTypeName, string serializedEvent)
        {
            var sql = String.Format("INSERT INTO {0} ({1}{2}, {3}, {4}) VALUES ({5}{6}, {7}, {8})",
                eventId != null ? Quote("Id") + ", " : String.Empty,
                Quote("Events"), Quote("UtcTimestamp"), Quote("EventTypeName"), Quote("SerializedEventData"),
                eventId != null ? GetDbParameterFullName("Id") + ", " : String.Empty,
                GetDbParameterFullName("UtcTimestamp"), GetDbParameterFullName("EventTypeName"), GetDbParameterFullName("SerializedEventData")
                );

            var parameters = new List<IDbDataParameter>();

            parameters.Add(CreateParameter(createDbDataParameter, "UtcTimestamp", DbType.DateTime, utcTimestamp));
            parameters.Add(CreateParameter(createDbDataParameter, "EventTypeName", DbType.String, eventTypeName));
            parameters.Add(CreateParameter(createDbDataParameter, "SerializedEventData", DbType.String, serializedEvent));

            return new SqlStatement(sql, parameters);
        }

        public virtual string Quote(string objectName)
        {
            return "[" + objectName + "]";
        }

        public virtual string GetDbParameterFullName(string dbParameterName)
        {
            return "@" + dbParameterName;
        }

        private IDbDataParameter CreateParameter(Func<IDbDataParameter> factory, string paramName, DbType type, object value)
        {
            var param = factory();
            param.ParameterName = paramName;
            param.DbType = type;
            param.Value = value;

            return param;
        }
    }
}

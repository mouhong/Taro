using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Taro.Events.Storage.Rdbms
{
    public interface ISqlStatementProvider
    {
        SqlStatement GetSelectStatement(
            Func<IDbDataParameter> createDbDataParameter, int fromId, int toId);

        SqlStatement GetSelectStatement(
            Func<IDbDataParameter> createDbDataParameter, DateTime? fromUtcTimestamp, DateTime? toUtcTimestamp);

        SqlStatement GetInsertStatement(
            Func<IDbDataParameter> createDbDataParameter, int? eventId, DateTime utcTimestamp, string eventTypeName, string serializedEvent);
    }
}

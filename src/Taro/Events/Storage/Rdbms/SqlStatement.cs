using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Taro.Events.Storage.Rdbms
{
    public class SqlStatement
    {
        public string Sql { get; set; }

        public IList<IDbDataParameter> Parameters { get; private set; }

        public SqlStatement(string sql)
        {
            Sql = sql;
            Parameters = new List<IDbDataParameter>();
        }

        public SqlStatement(string sql, IEnumerable<IDbDataParameter> parameters)
        {
            Sql = sql;
            Parameters = new List<IDbDataParameter>(parameters);
        }
    }
}

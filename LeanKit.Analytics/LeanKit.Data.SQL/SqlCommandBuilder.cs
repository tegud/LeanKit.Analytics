using System;
using System.Collections.Generic;
using System.Text;

namespace LeanKit.Data.SQL
{
    public class SqlCommandBuilder
    {
        private readonly StringBuilder _allSql;
        private readonly StringBuilder _orderSql = new StringBuilder();
        private SqlWhereClause _sqlWhereClause = SqlWhereClause.Empty;

        public SqlCommandBuilder(string select)
        {
            _allSql = new StringBuilder(select);
        }

        public SqlCommandAndParameters Build()
        {
            var whereClause = _sqlWhereClause.Build();

            return new SqlCommandAndParameters
                {
                    Sql = string.Concat(_allSql.ToString(), whereClause.Sql, _orderSql.ToString()),
                    Parameters = whereClause.Parameters
                };
        }

        public SqlOrderClause OrderBy(string column, SqlCommandOrderDirection desc)
        {
            var sqlFormatString = _orderSql.Length > 0 ? ", {0} {1}" : " ORDER BY {0} {1}";
            var direction = desc == SqlCommandOrderDirection.Descending ? "DESC" : "ASC";

            var orderBySql = string.Format(sqlFormatString, column, direction);
            _orderSql.Append(orderBySql);

            return new SqlOrderClause(this);
        }

        public SqlWhereClause Where(string whereClause, IDictionary<string, object> values = null)
        {
            _sqlWhereClause = new SqlWhereClause(whereClause, values);
            return _sqlWhereClause;
        }
    }

    public class SqlOrderClause
    {
        private readonly SqlCommandBuilder _command;

        public SqlOrderClause(SqlCommandBuilder command)
        {
            _command = command;
        }

        public SqlOrderClause ThenBy(string column, SqlCommandOrderDirection direction)
        {
            _command.OrderBy(column, direction);

            return this;
        }
    }

    public class SqlCommandAndParameters
    {
        public string Sql { get; set; }

        public IDictionary<string, object> Parameters { get; set; }
    }

    public enum SqlCommandOrderDirection
    {
        Descending,
        Ascending
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LeanKit.Data.SQL
{
    public class SqlCommandBuilder
    {
        private readonly StringBuilder _allSql;
        private readonly StringBuilder _orderSql = new StringBuilder();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        private const RegexOptions ParamRegexOptions = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
        private readonly Regex _parameterRegex = new Regex("@(?<parameterName>[a-z0-9]+)", ParamRegexOptions);

        public SqlCommandBuilder(string select)
        {
            _allSql = new StringBuilder(select);
        }

        public SqlCommandAndParameters Build()
        {
            return new SqlCommandAndParameters
                {
                    Sql = string.Concat( _allSql.ToString(), _orderSql.ToString()),
                    Parameters = _parameters
                };
        }

        public void OrderBy(string column, SqlCommandOrderDirection desc)
        {
            var sqlFormatString = _orderSql.Length > 0 ? ", {0} {1}" : " ORDER BY {0} {1}";
            var direction = desc == SqlCommandOrderDirection.Descending ? "DESC" : "ASC";

            var orderBySql = string.Format(sqlFormatString, column, direction);
            _orderSql.Append(orderBySql);
        }

        public void Where(string whereClause, params object[] values)
        {
            _allSql.Append(" WHERE " + whereClause);

            var matches = _parameterRegex.Matches(whereClause);
            var index = 0;

            foreach (Match match in matches)
            {
                var parameterName = match.Groups["parameterName"].Value;

                _parameters.Add(parameterName, GetValue(values, index));
                index++;
            }
        }

        private static object GetValue(object[] values, int index)
        {
            if (values == null)
            {
                return null;
            }

            return values[index];
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
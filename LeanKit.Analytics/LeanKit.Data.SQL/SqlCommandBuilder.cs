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

        private const RegexOptions PARAM_REGEX_OPTIONS = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
        private readonly Regex _parameterRegex = new Regex("@(?<parameterName>[a-z0-9]+)", PARAM_REGEX_OPTIONS);
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

        public void OrderBy(string column, SqlCommandOrderDirection desc)
        {
            var sqlFormatString = _orderSql.Length > 0 ? ", {0} {1}" : " ORDER BY {0} {1}";
            var direction = desc == SqlCommandOrderDirection.Descending ? "DESC" : "ASC";

            var orderBySql = string.Format(sqlFormatString, column, direction);
            _orderSql.Append(orderBySql);
        }

        public SqlWhereClause Where(string whereClause, IDictionary<string, object> values = null)
        {
            _sqlWhereClause = new SqlWhereClause(whereClause, values);
            return _sqlWhereClause;
        }
    }

    public class SqlWhereClause
    {
        private readonly StringBuilder _sql;
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private const RegexOptions PARAM_REGEX_OPTIONS = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
        private readonly Regex _parameterRegex = new Regex("@(?<parameterName>[a-z0-9]+)", PARAM_REGEX_OPTIONS);

        private SqlWhereClause()
        {
            _sql = new StringBuilder();
        }

        public SqlWhereClause(string whereClause, IDictionary<string, object> values)
        {
            _sql = new StringBuilder(" WHERE " + whereClause);

            var matches = ExtractParametersFromSql(whereClause);

            AddParameters(values, matches);
        }

        private MatchCollection ExtractParametersFromSql(string whereClause)
        {
            var matches = _parameterRegex.Matches(whereClause);
            return matches;
        }

        public SqlWhereClause And(string whereClause, IDictionary<string, object> values = null)
        {
            _sql.Append(" AND " + whereClause);

            var matches = ExtractParametersFromSql(whereClause);

            AddParameters(values, matches);

            return this;
        }

        public SqlWhereClause Parameters(Dictionary<string, object> dictionary)
        {
            foreach(var kvp in dictionary)
            {
                if (_parameters.ContainsKey(kvp.Key))
                {
                    _parameters[kvp.Key] = kvp.Value;
                }
                else
                {
                    _parameters.Add(kvp.Key, kvp.Value);
                }
            }
            return this;
        }

        private void AddParameters(IDictionary<string, object> values, MatchCollection matches)
        {
            foreach (Match match in matches)
            {
                var parameterName = match.Groups["parameterName"].Value;

                _parameters.Add(parameterName, GetValue(values, parameterName));
            }
        }

        private static object GetValue(IDictionary<string, object> values, string key)
        {
            if(values == null || !values.ContainsKey(key))
            {
                return null;
            }

            return values[key];
        }

        public static SqlWhereClause Empty = new SqlWhereClause();

        public SqlCommandAndParameters Build()
        {
            return new SqlCommandAndParameters
                {
                    Sql = _sql.ToString(),
                    Parameters = _parameters
                };
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
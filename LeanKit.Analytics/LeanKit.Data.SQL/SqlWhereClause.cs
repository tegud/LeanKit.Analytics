using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LeanKit.Data.SQL
{
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

        public SqlWhereClause And(string whereClause, IDictionary<string, object> values = null)
        {
            _sql.Append(" AND " + whereClause);

            var parameters = ExtractParametersFromSql(whereClause);

            AddParameters(values, parameters);

            return this;
        }

        public SqlWhereClause Or(string whereClause, Dictionary<string, object> values = null)
        {
            _sql.Append(" OR " + whereClause);

            var parameters = ExtractParametersFromSql(whereClause);

            AddParameters(values, parameters);

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

        private MatchCollection ExtractParametersFromSql(string whereClause)
        {
            var matches = _parameterRegex.Matches(whereClause);
            return matches;
        }

        private void AddParameters(IDictionary<string, object> values, MatchCollection matches)
        {
            foreach (Match match in matches)
            {
                var parameterName = match.Groups["parameterName"].Value;

                if(_parameters.ContainsKey(parameterName))
                {
                    _parameters[parameterName] = GetValue(values, parameterName);
                }
                else
                {
                    _parameters.Add(parameterName, GetValue(values, parameterName));   
                }
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
}
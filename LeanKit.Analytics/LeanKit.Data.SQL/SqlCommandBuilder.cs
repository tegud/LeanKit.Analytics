using System.Text;

namespace LeanKit.Data.SQL
{
    public class SqlCommandBuilder
    {
        private readonly StringBuilder _allSql;
        private readonly StringBuilder _orderSql = new StringBuilder();

        public SqlCommandBuilder(string select)
        {
            _allSql = new StringBuilder(select);
        }

        public SqlCommandAndParameters Build()
        {
            return new SqlCommandAndParameters
                {
                    Sql = string.Concat( _allSql.ToString(), _orderSql.ToString())
                };
        }

        public void OrderBy(string column, SqlCommandOrderDirection desc)
        {
            var sqlFormatString = _orderSql.Length > 0 ? ", {0} {1}" : " ORDER BY {0} {1}";
            var direction = desc == SqlCommandOrderDirection.Descending ? "DESC" : "ASC";

            var orderBySql = string.Format(sqlFormatString, column, direction);
            _orderSql.Append(orderBySql);
        }

        public void Where(string whereClause)
        {
            _allSql.Append(" WHERE " + whereClause);
        }
    }

    public class SqlCommandAndParameters
    {
        public string Sql { get; set; }
    }

    public enum SqlCommandOrderDirection
    {
        Descending,
        Ascending
    }
}
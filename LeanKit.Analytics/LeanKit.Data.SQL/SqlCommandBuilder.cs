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
            var orderBySql = string.Format(" ORDER BY {0} {1}", column, desc == SqlCommandOrderDirection.Descending ? "DESC" : "");
            _orderSql.Append(orderBySql);
        }
    }

    public class SqlCommandAndParameters
    {
        public string Sql { get; set; }
    }

    public enum SqlCommandOrderDirection
    {
        Descending
    }
}
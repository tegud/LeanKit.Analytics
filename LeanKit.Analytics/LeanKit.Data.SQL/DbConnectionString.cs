namespace LeanKit.Data.SQL
{
    public class DbConnectionString
    {
        public string ConnectionString { get; private set; }

        public DbConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
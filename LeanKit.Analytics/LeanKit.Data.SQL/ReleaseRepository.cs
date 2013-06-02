using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace LeanKit.Data.SQL
{
    public class ReleaseRepository : IGetReleasesFromTheDatabase
    {
        private readonly string _connectionString;

        public ReleaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ReleaseRecord> GetUpcomingReleases()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<ReleaseRecord>(@"SELECT * FROM Release R WHERE R.PlannedDate > GETDATE() ORDER BY R.PlannedDate ASC");
            }
        }
    }

    public interface IGetReleasesFromTheDatabase
    {
        IEnumerable<ReleaseRecord> GetUpcomingReleases();
    }

    public class ReleaseRecord
    {
        public int Id { get; set; }

        public string SvnRevision { get; set; }

        public DateTime PlannedDate { get; set; }
    }
}

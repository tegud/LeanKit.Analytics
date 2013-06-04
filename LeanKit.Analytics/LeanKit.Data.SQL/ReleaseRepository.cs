using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public void Create(ReleaseRecord newRelease)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var releaseId = sqlConnection.Query<int>(@"INSERT INTO Release (PlannedDate) SELECT @plannedStart; SELECT CAST(SCOPE_IDENTITY() as int)", new
                    {
                        plannedStart = newRelease.PlannedDate
                    }).First();

                foreach(var ticket in newRelease.IncludedTickets)
                {
                    sqlConnection.Execute(@"DELETE FROM ReleaseCard WHERE CardID = @CardID; INSERT INTO ReleaseCard (ReleaseID, CardID) SELECT @ReleaseID, @CardID", new
                    {
                        releaseId,
                        ticket.CardId
                    });
                }
            }
        }
    }

    public interface IGetReleasesFromTheDatabase
    {
        IEnumerable<ReleaseRecord> GetUpcomingReleases();
        void Create(ReleaseRecord newRelease);
    }

    public class ReleaseRecord
    {
        public int Id { get; set; }

        public string SvnRevision { get; set; }

        public DateTime PlannedDate { get; set; }

        public IEnumerable<IncludedTicketRecord> IncludedTickets { get; set; }
    }

    public class IncludedTicketRecord
    {
        public int CardId { get; set; }
    }
}

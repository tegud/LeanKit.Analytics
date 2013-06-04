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

                var releases = new List<ReleaseRecord>();

                sqlConnection.Query<ReleaseRecord, IncludedTicketRecord, ReleaseRecord>(@"SELECT R.*, RC.CardID FROM Release R LEFT OUTER JOIN ReleaseCard RC ON R.ID = RC.ReleaseID WHERE R.PlannedDate > GETDATE() ORDER BY R.PlannedDate ASC", (release, ticket) =>
                    {
                        var existingRelease = releases.FirstOrDefault(r => r.Id == release.Id);

                        if(existingRelease == null)
                        {
                            existingRelease = release;
                            releases.Add(release);
                        }

                        existingRelease.IncludedTickets.Add(ticket);

                        return release;
                    }, splitOn: "CardID");

                return releases;
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

        public List<IncludedTicketRecord> IncludedTickets { get; set; }

        public ReleaseRecord()
        {
            IncludedTickets = new List<IncludedTicketRecord>();
        }
    }

    public class IncludedTicketRecord
    {
        public int CardId { get; set; }
    }
}

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

        public ReleaseRepository(DbConnectionString connectionString)
        {
            _connectionString = connectionString.ConnectionString;
        }

        public ReleaseRecord GetRelease(int id)
        {
            var parameters = new Dictionary<string, object>
                {
                    {"id", id}
                };

            return GetListOfReleases(@"SELECT R.*, RC.CardID, C.ExternalID, C.Title, C.Size FROM Release R LEFT OUTER JOIN ReleaseCard RC ON R.ID = RC.ReleaseID LEFT OUTER JOIN Card C ON RC.CardID = C.ID WHERE R.ID = @ID",
                parameters).Single();
        }

        public void SetStartedDate(int id, DateTime started)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Execute("UPDATE Release SET StartedAt = @started WHERE ID = @ID AND StartedAt IS NULL", new { id, started });
            }
        }

        public void SetCompletedDate(int id, DateTime completed)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Execute("UPDATE Release SET CompletedAt = @completed WHERE ID = @ID", new { id, completed });
            }
        }

        public IEnumerable<ReleaseRecord> GetAllReleases()
        {
            return GetListOfReleases(@"SELECT R.*, RC.CardID FROM Release R LEFT OUTER JOIN ReleaseCard RC ON R.ID = RC.ReleaseID ORDER BY R.PlannedDate DESC");
        }

        public IEnumerable<ReleaseRecord> GetUpcomingReleases()
        {
            return GetListOfReleases(@"SELECT R.*, RC.CardID FROM Release R LEFT OUTER JOIN ReleaseCard RC ON R.ID = RC.ReleaseID WHERE R.PlannedDate > GETDATE() ORDER BY R.PlannedDate ASC");
        }

        private IEnumerable<ReleaseRecord> GetListOfReleases(string sql)
        {
            return GetListOfReleases(sql, new Dictionary<string, object>(0));
        }

        private IEnumerable<ReleaseRecord> GetListOfReleases(string sql, Dictionary<string, object> parameters)
        {
            var sqlParameters = new DynamicParameters();

            foreach (var param in parameters)
            {
                sqlParameters.Add(param.Key, param.Value);
            }

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var releases = new List<ReleaseRecord>();

                sqlConnection.Query<ReleaseRecord, IncludedTicketRecord, ReleaseRecord>(sql, (release, ticket) =>
                    {
                        var existingRelease = releases.FirstOrDefault(r => r.Id == release.Id);

                        if (existingRelease == null)
                        {
                            existingRelease = release;
                            releases.Add(release);
                        }

                        if (ticket != null)
                        {
                            existingRelease.IncludedTickets.Add(ticket);
                        }

                        return release;
                    }, splitOn: "CardID", param: sqlParameters);

                return releases;
            }
        }

        public int Create(ReleaseRecord newRelease)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var releaseId = sqlConnection.Query<int>(@"INSERT INTO Release (PlannedDate, SvnRevision, ServiceNowId) SELECT @plannedStart, @SvnRevision, @ServiceNowId; SELECT CAST(SCOPE_IDENTITY() as int)", new
                    {
                        plannedStart = newRelease.PlannedDate, 
                        newRelease.SvnRevision, 
                        newRelease.ServiceNowId
                    }).First();

                foreach (var ticket in newRelease.IncludedTickets)
                {
                    sqlConnection.Execute(@"DELETE FROM ReleaseCard WHERE CardID = @CardID; INSERT INTO ReleaseCard (ReleaseID, CardID) SELECT @ReleaseID, @CardID", new
                    {
                        releaseId,
                        ticket.CardId
                    });
                }

                return releaseId;
            }
        }

        public void Update(ReleaseRecord releaseRecord)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Execute(@"UPDATE Release 
                                        SET PlannedDate = @plannedStart, SvnRevision = @SvnRevision, ServiceNowId =  @ServiceNowId 
                                        WHERE ID = @ID; DELETE FROM ReleaseCard WHERE ReleaseID = @ID;",
                                      new
                                          {
                                              plannedStart = releaseRecord.PlannedDate,
                                              releaseRecord.SvnRevision,
                                              releaseRecord.ServiceNowId,
                                              releaseRecord.Id
                                          });

                foreach (var ticket in releaseRecord.IncludedTickets)
                {
                    sqlConnection.Execute(@"INSERT INTO ReleaseCard (ReleaseID, CardID) 
                                            SELECT @ID, @CardID", new
                    {
                        releaseRecord.Id,
                        ticket.CardId
                    });
                }
            }
        }
    }

    public interface IGetReleasesFromTheDatabase
    {
        IEnumerable<ReleaseRecord> GetUpcomingReleases();

        int Create(ReleaseRecord newRelease);
        void Update(ReleaseRecord releaseRecord);

        IEnumerable<ReleaseRecord> GetAllReleases();
        ReleaseRecord GetRelease(int id);
        void SetStartedDate(int id, DateTime started);
        void SetCompletedDate(int id, DateTime completed);
    }

    public class ReleaseRecord
    {
        public int Id { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }

        public DateTime PlannedDate { get; set; }

        public int PlannedDuration { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public List<IncludedTicketRecord> IncludedTickets { get; set; }

        public ReleaseRecord()
        {
            IncludedTickets = new List<IncludedTicketRecord>();
        }
    }

    public class IncludedTicketRecord
    {
        public int CardId { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public int Size { get; set; }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace LeanKit.Data.SQL
{
    public interface IGetActivitiesFromTheDatabase
    {
        void SaveActivities(IEnumerable<Activity> activities);
        IEnumerable<Activity> GetLanes();
    }

    public class ActivityRepository : IGetActivitiesFromTheDatabase
    {
        private readonly string _connectionString;

        public ActivityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveActivities(IEnumerable<Activity> activities)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                foreach (var activity in activities)
                {
                    AddActivity(sqlConnection, activity);
                }
            }
        }

        public IEnumerable<Activity> GetLanes()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<Activity>("SELECT * FROM BoardLane ORDER BY [Index] ASC");
            }
        }

        private static void AddActivity(IDbConnection connection, Activity activity)
        {
            connection.Execute(@"IF EXISTS (SELECT * FROM BoardLane WHERE ID = @ID)
                                BEGIN 
                                    UPDATE BoardLane SET Title = @Title, [Index] = @Index WHERE ID = @ID
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO BoardLane(ID, Title, [Index]) SELECT @ID, @Title, @Index
                                END", new { activity.Id, activity.Title, activity.Index });
        }
    }
}
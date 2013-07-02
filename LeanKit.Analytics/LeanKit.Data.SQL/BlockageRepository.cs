using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace LeanKit.Data.SQL
{
    public class BlockageRepository : IGetBlockagesFromTheDatabase
    {
        private readonly string _connectionString;

        public BlockageRepository(DbConnectionString connectionString)
        {
            _connectionString = connectionString.ConnectionString;
        }

        public IEnumerable<TicketBlockage> Get(CycleTimeQuery query)
        {
            var command = new SqlCommandBuilder(@"  SELECT  *
                                                    FROM    CardBlockage CB
                                                            INNER JOIN Card C ON CB.CardID = C.ID");
            var where = command.Where("C.Started IS NOT NULL");

            if(query.Start > DateTime.MinValue && query.End > DateTime.MinValue)
            {
                where.And("(CB.Started BETWEEN @Start AND @End AND (CB.Finished IS NULL OR CB.Finished BETWEEN @Start AND @End))",
                          new Dictionary<string, object>
                              {
                                  { "Start", query.Start },
                                  { "End", query.End }
                              });
            }

            var builtCommand = command.Build();

            var sqlParameters = new DynamicParameters();

            foreach (var param in builtCommand.Parameters)
            {
                sqlParameters.Add(param.Key, param.Value);
            }

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                return sqlConnection.Query<TicketBlockage>(builtCommand.Sql, sqlParameters);
            }
        }
    }
}
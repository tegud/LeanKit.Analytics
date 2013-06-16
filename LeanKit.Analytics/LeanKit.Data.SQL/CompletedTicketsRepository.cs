using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace LeanKit.Data.SQL
{
    public class CompletedTicketsRepository : IGetReleasedTicketsFromTheDatabase
    {
        private readonly string _connectionString;
        private readonly ICreateTickets _ticketFactory;

        public CompletedTicketsRepository(DbConnectionString connectionString, ICreateTickets ticketFactory)
        {
            _connectionString = connectionString.ConnectionString;
            _ticketFactory = ticketFactory;
        }

        public IEnumerable<Ticket> Get(CycleTimeQuery query)
        {
            var tickets = new List<TicketRecord>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Query<TicketRecord, TicketActivityRecord, TicketReleaseRecord, TicketRecord>(
                    @"SELECT C.*, CA.*, U.Name AssignedUserName, U.Email AssignedUserEmail, R.*
                            FROM CardActivity CA 
                                INNER JOIN Card C ON CA.CardID = C.ID
                                LEFT OUTER JOIN LeanKitUser U ON CA.AssignedUserID = U.ID
                                LEFT OUTER JOIN ReleaseCard RC ON C.ID = RC.CardID
                                LEFT OUTER JOIN Release R ON RC.ReleaseID = R.ID
                            WHERE C.Finished IS NOT NULL
                            AND (@Started IS NULL OR C.Finished >= @Started)
                            AND (@Finished IS NULL OR C.Finished <= @Finished)
                            ORDER BY C.Finished DESC",
                    (ticket, activity, release) =>
                        {
                            var existingTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);

                            var currentTicket = existingTicket ?? ticket;

                            if (existingTicket == null)
                            {
                                ticket.Release = release;
                                tickets.Add(ticket);
                            }

                            currentTicket.Activities.Add(activity);

                            return ticket;
                        },
                    new
                        {
                            Started = query.Start > DateTime.MinValue ? (object)query.Start : null,
                            Finished = query.End > DateTime.MinValue ? (object) query.End.AddDays(1).AddSeconds(-1) : null
                        });
            }

            return tickets.Select(_ticketFactory.Build);
        }
    }
}
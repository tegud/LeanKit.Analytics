using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace LeanKit.Data.SQL
{
    public interface IGetReleasedTicketsFromTheDatabase
    {
        IEnumerable<Ticket> Get(CycleTimeQuery query);
    }

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

    public class TicketsRepository : ITicketRepository
    {
        private readonly string _connectionString;
        private readonly ICreateTickets _ticketFactory;

        public TicketsRepository(DbConnectionString connectionString, ICreateTickets ticketFactory)
        {
            _connectionString = connectionString.ConnectionString;
            _ticketFactory = ticketFactory;
        }

        public AllTicketsForBoard GetAll()
        {
            var tickets = new List<TicketRecord>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Query<TicketRecord, TicketActivityRecord, TicketRecord>(
                        @"SELECT C.*, CA.*, U.Name AssignedUserName, U.Email AssignedUserEmail 
                            FROM CardActivity CA 
                                INNER JOIN Card C ON CA.CardID = C.ID
                                LEFT OUTER JOIN LeanKitUser U ON CA.AssignedUserID = U.ID
                            ORDER BY C.ID, CA.ID",
                                                 (ticket, activity) =>
                                                 {
                                                     var existingTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);

                                                     var currentTicket = existingTicket ?? ticket;

                                                     if (existingTicket == null)
                                                     {
                                                         tickets.Add(ticket);
                                                     }

                                                     currentTicket.Activities.Add(activity);

                                                     return ticket;
                                                 });
            }

            return new AllTicketsForBoard
                {
                    Tickets = tickets.Select(_ticketFactory.Build).ToList()
                };
        }

        public void Save(Ticket ticket)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                const string ticketSql = @"IF EXISTS(SELECT ID FROM Card WHERE ID = @ID)
                                BEGIN
                                    UPDATE Card 
                                    SET Title = @Title, 
                                        ExternalId = @ExternalId,
                                        Size = @Size,
                                        Started = @Started,
                                        Finished = @Finished
                                    WHERE ID = @Id;
                                END
                                ELSE
                                BEGIN
                                    INSERT Card(ID, Title, ExternalId, Started, Finished, Size) values (@Id, @Title, @ExternalId, @Started, @Finished, @Size);
                                END";

                sqlConnection.Execute(ticketSql,
                                    new
                                    {
                                        ticket.Id,
                                        ticket.Title,
                                        ticket.ExternalId,
                                        Size = ticket.Size > 0 ? (int?) ticket.Size : null,
                                        Started = ticket.Started > DateTime.MinValue ? (DateTime?)ticket.Started : null,
                                        Finished = ticket.Finished > DateTime.MinValue ? (DateTime?)ticket.Finished : null
                                    });

                foreach (var activity in ticket.Activities)
                {
                    const string activitySql = @"IF @UserID IS NOT NULL AND NOT EXISTS (SELECT ID FROM LeanKitUser WHERE ID = @UserID)
                                        BEGIN
                                            INSERT INTO LeanKitUser (ID, Name, Email)
                                            VALUES (@UserID, @UserName, @UserEmail)
                                        END
                                        
                                        IF NOT EXISTS(SELECT ID FROM CardActivity WHERE CardID = @ID AND Activity = @Title AND Date = @Started)
                                        BEGIN
                                            INSERT INTO CardActivity(CardID, Activity, Date, AssignedUserID) 
                                            VALUES (@Id, @Title, @Started, @UserID)
                                        END";

                    sqlConnection.Execute(activitySql,
                                        new
                                        {
                                            ticket.Id,
                                            activity.Title,
                                            activity.Started,
                                            UserId = activity.AssignedUser != TicketActivityAssignedUser.UnAssigned
                                             ? (int?)activity.AssignedUser.Id : null,
                                            UserName = activity.AssignedUser != TicketActivityAssignedUser.UnAssigned
                                             ? activity.AssignedUser.Name : null,
                                            UserEmail =  activity.AssignedUser != TicketActivityAssignedUser.UnAssigned
                                             ? activity.AssignedUser.Email.Address : null
                                        });
                }
            }
        }
    }
}
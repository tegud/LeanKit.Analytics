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

                sqlConnection.Query<TicketRecord, TicketActivityRecord, TicketAssignedUserRecord, TicketBlockedRecord, TicketRecord>(
                        @"SELECT C.*, CA.*, U.Name AssignedUserName, U.Email AssignedUserEmail , AU.*, B.*
                            FROM CardActivity CA 
                                INNER JOIN Card C ON CA.CardID = C.ID
                                LEFT OUTER JOIN LeanKitUser U ON CA.AssignedUserID = U.ID
                                LEFT OUTER JOIN CardAssignedUsers CAU ON C.ID = CAU.CardID
                                LEFT OUTER JOIN LeanKitUser AU ON CAU.LeanKitUserID = AU.ID
                                LEFT OUTER JOIN CardBlockage B ON B.CardID = C.ID
                            ORDER BY C.ID, CA.ID",
                                                 (ticket, activity, assignedUser, blockage) =>
                                                 {
                                                     var existingTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);

                                                     var currentTicket = existingTicket ?? ticket;

                                                     if (existingTicket == null)
                                                     {
                                                         tickets.Add(ticket);
                                                     }

                                                     if (activity != null && !currentTicket.Activities.Any(a => a.Date == activity.Date && a.Activity == activity.Activity))
                                                     {
                                                         currentTicket.Activities.Add(activity);
                                                     }

                                                     if (assignedUser != null && !currentTicket.AssignedUsers.Any(u => u.Id == assignedUser.Id))
                                                     {
                                                         currentTicket.AssignedUsers.Add(assignedUser);
                                                     }

                                                     if(blockage != null && !currentTicket.Blockages.Any(b => b.Started == blockage.Started))
                                                     {
                                                         currentTicket.Blockages.Add(blockage);
                                                     }

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
                                    
                                    DELETE FROM CardAssignedUsers WHERE CardID = @ID
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
                                            UserId = activity.AssignedUser != TicketAssignedUser.UnAssigned
                                             ? (int?)activity.AssignedUser.Id : null,
                                            UserName = activity.AssignedUser != TicketAssignedUser.UnAssigned
                                             ? activity.AssignedUser.Name : null,
                                            UserEmail =  activity.AssignedUser != TicketAssignedUser.UnAssigned
                                             ? activity.AssignedUser.Email.Address : null
                                        });
                }

                foreach(var blockage in ticket.Blockages)
                {
                    sqlConnection.Execute(@"IF EXISTS(SELECT * FROM CardBlockage WHERE CardID = @CardID AND Started = @Started )
                                            BEGIN
                                                IF(@Finished IS NOT NULL)
                                                BEGIN
                                                    UPDATE  CardBlockage
                                                    SET     Finished = @Finished
                                                    WHERE   CardID = @CardID AND Started = @Started 
                                                END
                                            END
                                            ELSE
                                            BEGIN
                                                INSERT INTO CardBlockage (CardID, Reason, Started, Finished)
                                                VALUES (@CardID, @Reason, @Started, @Finished)
                                            END",
                                        new
                                        {
                                            CardID = ticket.Id,
                                            blockage.Reason,
                                            blockage.Started,
                                            Finished = blockage.Finished > DateTime.MinValue ? (DateTime?)blockage.Finished : null
                                        });
                }

                foreach(var userId in ticket.AssignedUsers.Select(u => u.Id).Distinct())
                {
                    sqlConnection.Execute(@"INSERT INTO CardAssignedUsers(CardID, LeanKitUserID)
                                            VALUES (@CardID, @UserID)",
                                          new
                                              {
                                                  CardId = ticket.Id,
                                                  UserID = userId
                                              });

                }
            }
        }
    }

    public class TicketAssignedUserRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
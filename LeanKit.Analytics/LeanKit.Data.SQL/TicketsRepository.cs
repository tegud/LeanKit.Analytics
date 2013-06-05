using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace LeanKit.Data.SQL
{
    public class TicketsRepository : ITicketRepository
    {
        private readonly string _connectionString;
        private readonly ICreateTickets _ticketFactory;

        public TicketsRepository(string connectionString, ICreateTickets ticketFactory)
        {
            _connectionString = connectionString;
            _ticketFactory = ticketFactory;
        }

        public AllTicketsForBoard GetAll()
        {
            var tickets = new List<TicketRecord>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Query<TicketRecord, TicketActivityRecord, TicketRecord>(@"SELECT C.*, CA.* FROM CardActivity CA INNER JOIN Card C ON CA.CardID = C.ID ORDER BY C.ID, CA.ID", (ticket, activity) =>
                    {
                        var existingTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);

                        var currentTicket = existingTicket ?? ticket;

                        if(existingTicket == null)
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

                var ticketSql = @"IF EXISTS(SELECT ID FROM Card WHERE ID = @ID)
                                BEGIN
                                    UPDATE Card SET Title = @Title, ExternalId = @ExternalId WHERE ID = @Id;
                                END
                                ELSE
                                BEGIN
                                    INSERT Card(ID, Title, ExternalId) values (@Id, @Title, @ExternalId);
                                END";

                sqlConnection.Execute(ticketSql,
                                    new
                                    {
                                        ticket.Id,
                                        ticket.Title,
                                        ticket.ExternalId
                                    });

                foreach(var activity in ticket.Activities)
                {
                    var activitySql = string.Format("IF NOT EXISTS(SELECT ID FROM CardActivity WHERE CardID = @ID AND Activity = @Title AND Date = @Started){0}BEGIN{0}INSERT INTO CardActivity(CardID, Activity, Date) VALUES (@Id, @Title, @Started){0}END", Environment.NewLine);

                    sqlConnection.Execute(activitySql,
                                        new
                                        {
                                            ticket.Id,
                                            activity.Title,
                                            activity.Started
                                        });
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using LeanKit.APIClient.API;
using LeanKit.Data.Activities;

namespace LeanKit.Data.Repositories
{
    public class TicketsRepository : ITicketRepository
    {
        private readonly string _connectionString;

        public TicketsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AllTicketsForBoard GetAll()
        {
            var tickets = new List<Ticket>();
            var ticketHistories = new Dictionary<int, List<LeanKitCardHistory>>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                sqlConnection.Query<Ticket, LeanKitCardHistory, Ticket>(@"SELECT * FROM CardActivity CA INNER JOIN Card C ON CA.CardID = C.ID ORDER BY C.ID, CA.ID", (ticket, activity) =>
                    {
                        if(tickets.Any(t => t.Id == ticket.Id))
                        {
                            ticketHistories[ticket.Id].Add(activity);
                        }
                        else
                        {
                            tickets.Add(ticket);
                            ticketHistories.Add(ticket.Id, new List<LeanKitCardHistory> { activity });
                        }



                        return ticket;
                    });
            }

            return new AllTicketsForBoard
                {
                    Tickets = tickets
                };
        }

        public void Save(Ticket ticket)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var ticketSql = string.Format(@"IF EXISTS(SELECT ID FROM Card WHERE ID = @ID){0}BEGIN{0}UPDATE{0}Card SET Title = @Title WHERE ID = @Id;{0}END{0}ELSE{0}BEGIN{0}INSERT Card(ID, Title) values (@Id, @Title);{0}END{0}", Environment.NewLine);

                sqlConnection.Execute(ticketSql,
                                    new
                                    {
                                        ticket.Id,
                                        ticket.Title
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
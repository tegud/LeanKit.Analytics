using System.Data.SqlClient;
using System.Linq;
using Dapper;
using LeanKit.Data.SQL.ErrorHandling;

namespace LeanKit.Data.SQL
{
    public class FullTicketInformationRepository : IGetAllTicketInformation
    {
        private readonly string _connectionString;
        private readonly ICreateTickets _ticketFactory;

        public FullTicketInformationRepository(DbConnectionString connectionString, ICreateTickets ticketFactory)
        {
            _connectionString = connectionString.ConnectionString;
            _ticketFactory = ticketFactory;
        }

        public Ticket Get(int id)
        {
            TicketRecord ticket = null;

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
                            WHERE C.ID = @CardID
                            ORDER BY C.ID, CA.ID",
                    (currentTicket, activity, assignedUser, blockage) =>
                        {
                            if (ticket == null)
                            {
                                ticket = currentTicket;
                            }

                            if (activity != null && !ticket.Activities.Any(a => a.Date == activity.Date && a.Activity == activity.Activity))
                            {
                                ticket.Activities.Add(activity);
                            }

                            if (assignedUser != null && !ticket.AssignedUsers.Any(u => u.Id == assignedUser.Id))
                            {
                                ticket.AssignedUsers.Add(assignedUser);
                            }

                            if (blockage != null && !string.IsNullOrWhiteSpace(blockage.Reason) && !ticket.Blockages.Any(b => b.Started == blockage.Started))
                            {
                                ticket.Blockages.Add(blockage);
                            }

                            return currentTicket;
                        },
                    new { CardID = id });

                if(ticket == null)
                {
                    throw new TicketNotFoundInDatabaseException();
                }

                return _ticketFactory.Build(ticket);
            }
        }
    }
}
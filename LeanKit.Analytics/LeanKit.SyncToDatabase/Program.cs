using System;
using LeanKit.APIClient.API;
using LeanKit.Data;
using LeanKit.Data.API;
using LeanKit.Data.SQL;
using TicketFactory = LeanKit.Data.API.TicketFactory;

namespace LeanKit.SyncToDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "";
            const string password = "";
            const string account = "";
            const string boardId = "";
            const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=LeanKitSync;Persist Security Info=True;User ID=carduser;Password=password;MultipleActiveResultSets=True";

            var apiCaller = new ApiCaller
                {
                    Account = account,
                    BoardId = boardId,
                    Credentials = new ApiCredentials
                        {
                            Username = username,
                            Password = password
                        }
                };

            var workDurationFactory = new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9,
                    End =  17
                });
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var ticketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, ticketActivityFactory);
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var ticketFactory = new TicketFactory(ticketActivitiesFactory, new TicketCycleTimeDurationFactory(workDurationFactory), new TicketStartDateFactory(activityIsInProgressSpecification), new TicketFinishDateFactory(activityIsLiveSpecification));
            var allTickets = new AllBoardTicketsFromApi(apiCaller, ticketFactory).Get().Tickets;
            var ticketRepository = new TicketsRepository(connectionString);

            foreach(var ticket in allTickets)
            {
                ticketRepository.Save(ticket);
            }
        }
    }
}

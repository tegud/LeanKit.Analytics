using System;
using LeanKit.APIClient.API;
using LeanKit.Data;
using LeanKit.Data.API;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateTime;
using TicketActivityFactory = LeanKit.Data.API.TicketActivityFactory;

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
            var activityIsLiveSpecification = new ActivityIsLiveSpecification();

            var dateTimeWrapper = new DateTimeWrapper();
            var ticketCycleTimeDurationFactory = new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper);
            var ticketStartDateFactory = new TicketStartDateFactory(activityIsInProgressSpecification);
            var ticketFinishDateFactory = new TicketFinishDateFactory(activityIsLiveSpecification);

            var apiTicketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var apiTicketActivitiesFactory = new TicketActivitiesFactory(apiCaller, apiTicketActivityFactory);
            var apiTicketFactory = new Data.API.TicketFactory(apiTicketActivitiesFactory, ticketCycleTimeDurationFactory, ticketStartDateFactory, ticketFinishDateFactory);

            var allTickets = new AllBoardTicketsFromApi(apiCaller, apiTicketFactory, new ValidArchiveCardSpecification()).Get().Tickets;

            var sqlTicketActivityFactory = new Data.SQL.TicketActivityFactory(workDurationFactory);

            var sqlTicketFactory = new Data.SQL.TicketFactory(workDurationFactory, ticketStartDateFactory, ticketFinishDateFactory, sqlTicketActivityFactory, ticketCycleTimeDurationFactory);
            var ticketRepository = new TicketsRepository(connectionString, sqlTicketFactory);

            foreach(var ticket in allTickets)
            {
                ticketRepository.Save(ticket);
            }
        }
    }
}

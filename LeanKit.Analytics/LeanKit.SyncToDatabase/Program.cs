using System;
using System.Collections;
using System.Configuration;
using LeanKit.APIClient.API;
using LeanKit.Data;
using LeanKit.Data.API;
using LeanKit.Data.SQL;
using LeanKit.Utilities.Collections;
using LeanKit.Utilities.DateAndTime;
using TicketActivityFactory = LeanKit.Data.API.TicketActivityFactory;

namespace LeanKit.SyncToDatabase
{
    class Program
    {

        static void Main(string[] args)
        {
            var apiSettingsSection = (Hashtable)ConfigurationManager.GetSection("ApiSettings");

            var username = apiSettingsSection.GetValue("username");
            var password = apiSettingsSection.GetValue("password");
            var account = apiSettingsSection.GetValue("account");
            var boardId = apiSettingsSection.GetValue("boardId");

            var connectionString = new DbConnectionString(ConfigurationManager.ConnectionStrings["LeanKitSyncDb"].ConnectionString);

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

            var workDurationFactory = new WorkDurationFactory(new DateTime[0], new WorkDayDefinition { Start = 9, End =  17 });
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var activityIsLiveSpecification = new ActivityIsLiveSpecification();
            var validArchiveCardSpecification = new ValidArchiveCardSpecification();

            var dateTimeWrapper = new DateTimeWrapper();
            var ticketCycleTimeDurationFactory = new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper);
            var ticketStartDateFactory = new TicketStartDateFactory(activityIsInProgressSpecification);
            var ticketFinishDateFactory = new TicketFinishDateFactory(activityIsLiveSpecification);

            var apiTicketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var apiTicketActivitiesFactory = new TicketActivitiesFactory(apiCaller, apiTicketActivityFactory);
            var apiTicketFactory = new Data.API.TicketFactory(apiTicketActivitiesFactory, ticketCycleTimeDurationFactory, ticketStartDateFactory, ticketFinishDateFactory);

            var sqlTicketActivityFactory = new Data.SQL.TicketActivityFactory(workDurationFactory);
            var ticketCurrentActivityFactory = new CurrentActivityFactory();

            var sqlTicketFactory = new Data.SQL.TicketFactory(ticketStartDateFactory, ticketFinishDateFactory, sqlTicketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);
            var ticketRepository = new TicketsRepository(connectionString, sqlTicketFactory);
            var activityRepository = new ActivityRepository(connectionString);

            var board = new AllBoardTicketsFromApi(apiCaller, apiTicketFactory, validArchiveCardSpecification).Get();
            var allTickets = board.Tickets;

            activityRepository.SaveActivities(board.Lanes);

            foreach(var ticket in allTickets)
            {
                ticketRepository.Save(ticket);
            }
        }
    }
}

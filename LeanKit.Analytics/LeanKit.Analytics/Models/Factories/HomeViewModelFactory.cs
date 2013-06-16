using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Analytics.Controllers;
using LeanKit.Analytics.Models.ViewModels;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.Analytics.Models.Factories
{
    public class HomeViewModelFactory : IHomeViewModelFactory
    {
        public HomeViewModel Build()
        {
            const string connectionString = @"Data Source=.\Express2008;Initial Catalog=LeanKitSync;Persist Security Info=True;User ID=carduser;Password=password;MultipleActiveResultSets=True";

            var workDurationFactory = new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9, 
                    End = 17
                });
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();

            var dateTimeWrapper = new DateTimeWrapper();
            var ticketCycleTimeDurationFactory = new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper);
            var ticketStartDateFactory = new TicketStartDateFactory(activityIsInProgressSpecification);
            var ticketFinishDateFactory = new TicketFinishDateFactory(activityIsLiveSpecification);
            var sqlTicketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var sqlTicketCurrentActivityFactory = new CurrentActivityFactory();
            var ticketBlockagesFactory = new TicketBlockageFactory(workDurationFactory);

            var sqlTicketFactory = new TicketFactory(ticketStartDateFactory, ticketFinishDateFactory, sqlTicketActivityFactory, ticketCycleTimeDurationFactory, sqlTicketCurrentActivityFactory, ticketBlockagesFactory);
            var ticketRepository = new TicketsRepository(new DbConnectionString(connectionString), sqlTicketFactory);
            var allTickets = ticketRepository.GetAll();

            var allTicketActivityDurations = allTickets.Tickets.SelectMany(t => t.Activities.Select(a => new
                {
                    a.Duration, 
                    Title = MapActivityTitle(a.Title)
                }))
                .Where(x => !string.IsNullOrWhiteSpace(x.Title) && x.Duration.Hours > 0).ToArray();

            var totalActivityHours = (double)allTicketActivityDurations.Sum(t => t.Duration.Hours);

            var wasteGraphList = new List<WasteGraphActivity>
                {
                    new WasteGraphActivity { Activity = "Developing" },
                    new WasteGraphActivity { Activity = "Testing" },
                    new WasteGraphActivity { Activity = "Waiting to Test",IsWaste = true },
                    new WasteGraphActivity { Activity = "Waiting to Release", IsWaste = true },
                    new WasteGraphActivity { Activity = "Blocked", IsWaste = true }
                };

            foreach(var ticketActivity in allTicketActivityDurations)
            {
                var wasteGraphActivity = wasteGraphList.FirstOrDefault(a => a.Activity == ticketActivity.Title);

                if (wasteGraphActivity == null)
                {
                    continue;
                }

                wasteGraphActivity.Percent += ((ticketActivity.Duration.Hours/totalActivityHours)*100);
            }

            return new HomeViewModel
                {
                    MainWasteGraph = new WasteGraph(wasteGraphList)
                };
        }

        private static string MapActivityTitle(string title)
        {
            if(title.StartsWith("Blocked: "))
            {
                return "Blocked";
            }

            var titleMappings = new Dictionary<string, string>
                {
                    {"DEV WIP", "Developing"},
                    {"DEV Done", "Waiting to Test"},
                    {"READY FOR TEST", "Waiting to Test"},
                    {"TEST WIP", "Testing" },
                    {"READY FOR RELEASE", "Waiting to Release"}
                };


            title = title.ToUpper();
            return titleMappings.ContainsKey(title) ? titleMappings[title] : string.Empty;
        }
    }
}
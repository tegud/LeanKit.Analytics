using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Analytics.Controllers;
using LeanKit.Analytics.Models.ViewModels;
using LeanKit.Data;
using LeanKit.Data.Activities;
using LeanKit.Data.Repositories;

namespace LeanKit.Analytics.Models.Factories
{
    public class HomeViewModelFactory : IHomeViewModelFactory
    {
        public HomeViewModel Build()
        {
            const string username = "";
            const string password = "";
            const string account = "";
            const string boardID = "";

            var apiCaller = new ApiCaller
            {
                Account = account,
                BoardId = boardID,
                Credentials = new ApiCredentials
                {
                    Username = username,
                    Password = password
                }
            };

            var workDurationFactory = new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9, 
                    End = 17
                });
            var ticketActivitiesFactory = new TicketActivitiesFactory(apiCaller, new TicketActivityFactory(workDurationFactory));
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            var allTickets = new AllBoardTicketsFromApi(apiCaller, new TicketFactory(ticketActivitiesFactory,
                                                       workDurationFactory,
                                                       activityIsInProgressSpecification)).Get();

            var allTicketActivityDurations = allTickets.Tickets.SelectMany(t => t.Activities.Select(a => new
                {
                    a.Duration, 
                    Title = MapActivityTitle(a.Title)
                }))
                .Where(x => !string.IsNullOrWhiteSpace(x.Title) && x.Duration.Hours > 0).ToArray();

            var totalActivityHours = (double)allTicketActivityDurations.Sum(t => t.Duration.Hours);

            var wasteGraphList = new List<WasteGraphActivity>
                {
                    new WasteGraphActivity {Activity = "Developing"},
                    new WasteGraphActivity {Activity = "Testing"},
                    new WasteGraphActivity {Activity = "Waiting to Test",IsWaste = true},
                    new WasteGraphActivity {Activity = "Waiting to Release", IsWaste = true},
                    new WasteGraphActivity {Activity = "Blocked", IsWaste = true}
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Analytics.Models.ViewModels;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class TicketController : Controller
    {
        public ActionResult Index(int id)
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

            var sqlTicketFactory = new TicketFactory(ticketStartDateFactory, ticketFinishDateFactory, sqlTicketActivityFactory, ticketCycleTimeDurationFactory, sqlTicketCurrentActivityFactory);
            var ticketRepository = new TicketsRepository(new DbConnectionString(connectionString), sqlTicketFactory);

            var ticket = ticketRepository.GetAll().Tickets.First(t => t.Id == id);

            var allTicketActivityDurations = ticket.Activities.Select(a => new
                {
                    a.Duration,
                    Title = MapActivityTitle(a.Title)
                }).Where(y => !string.IsNullOrWhiteSpace(y.Title) && y.Duration.Hours > 0);

            var totalActivityHours = (double)allTicketActivityDurations.Sum(t => t.Duration.Hours);

            var wasteGraphList = new List<WasteGraphActivity>
                {
                    new WasteGraphActivity { Activity = "Developing" },
                    new WasteGraphActivity { Activity = "Testing" },
                    new WasteGraphActivity { Activity = "Waiting to Test",IsWaste = true },
                    new WasteGraphActivity { Activity = "Waiting to Release", IsWaste = true },
                    new WasteGraphActivity { Activity = "Blocked", IsWaste = true }
                };

            foreach (var ticketActivity in allTicketActivityDurations)
            {
                var wasteGraphActivity = wasteGraphList.FirstOrDefault(a => a.Activity == ticketActivity.Title);

                if (wasteGraphActivity == null)
                {
                    continue;
                }

                wasteGraphActivity.Percent += ((ticketActivity.Duration.Hours / totalActivityHours) * 100);
            }

            var x = new WasteGraph(wasteGraphList);

            return View(x);
        }

        private static string MapActivityTitle(string title)
        {
            if (title.StartsWith("Blocked: "))
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

    public class TicketViewModel
    {

    }
}

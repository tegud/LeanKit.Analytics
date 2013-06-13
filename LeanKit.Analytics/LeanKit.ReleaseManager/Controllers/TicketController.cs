using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

            var activityBreakdownItems = new List<ActivityBreakdownItem>
                {
                    new ActivityBreakdownItem { Activity = "Developing" },
                    new ActivityBreakdownItem { Activity = "Testing" },
                    new ActivityBreakdownItem { Activity = "Waiting to Test",IsWaste = true },
                    new ActivityBreakdownItem { Activity = "Waiting to Release", IsWaste = true },
                    new ActivityBreakdownItem { Activity = "Blocked", IsWaste = true }
                };

            foreach (var ticketActivity in allTicketActivityDurations)
            {
                var activityName = ticketActivity.Title;
                if (activityName.Contains("Blocked"))
                {
                    activityName = "Blocked";
                }

                var activity = activityBreakdownItems.FirstOrDefault(a => a.Activity == activityName);

                if (activity == null)
                {
                    continue;
                }

                activity.Percent += ((ticketActivity.Duration.Hours / totalActivityHours) * 100);
                activity.Hours += ticketActivity.Duration.Hours;
                activity.Days += ticketActivity.Duration.Days;
            }

            var activityBreakdown = new ActivityBreakdown(activityBreakdownItems);

            return View(new TicketViewModel
                {
                    ActivityBreakdown = activityBreakdown,
                    Title = ticket.Title,
                    ExternalId = ticket.ExternalId,
                    CurrentStatus = GetTicketCurrentStatus(ticket),
                    Started = DateFriendlyText(ticket.Started),
                    Finished = DateFriendlyText(ticket.Finished)
                });
        }

        private static string DateFriendlyText(DateTime date)
        {
            if (date > DateTime.MinValue)
            {
                return date.ToFriendlyText("dd MMM yyyy", " \"at\" HH:mm");
            }

            return "";
        }

        private static string GetTicketCurrentStatus(Ticket ticket)
        {
            if (ticket.Finished > DateTime.MinValue)
            {
                return "Live";
            }

            return ticket.CurrentActivity.Title;
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
        public ActivityBreakdown ActivityBreakdown { get; set; }

        public string Title { get; set; }

        public string ExternalId { get; set; }

        public string CurrentStatus { get; set; }

        public string Started { get; set; }

        public string Finished { get; set; }
    }

    public class ActivityBreakdown
    {
        public IEnumerable<ActivityBreakdownItem> Activities { get; private set; }

        public ActivityBreakdown(IEnumerable<ActivityBreakdownItem> activities)
        {
            Activities = activities;
        }
    }

    public class ActivityBreakdownItem
    {
        public string Activity { get; set; }

        public string TimeSummary
        {
            get
            {
                return string.Format("{0} hr{1}", Hours, Hours != 1 ? "s" : "");
            }
        }

        public int Days { get; set; }

        public int Hours { get; set; }

        public double Percent { get; set; }

        public string FormattedPercent { get { return Percent.ToString("0.00"); } }

        public bool IsWaste { get; set; }

        public string ClassName
        {
            get { return Activity.Replace(" ", string.Empty); }
        }
    }
}

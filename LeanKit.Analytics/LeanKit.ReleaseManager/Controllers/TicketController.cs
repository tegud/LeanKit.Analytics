using System;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models.Graphs;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class TicketController : Controller
    {
        public ActionResult Index(int id)
        {
            string connectionString = MvcApplication.ConnectionString.ConnectionString;

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

            var activityBreakdown = new ActivityBreakDownFactory().Build(ticket);

            return View(new TicketViewModel
                {
                    ActivityBreakdown = activityBreakdown,
                    Title = ticket.Title,
                    ExternalId = ticket.ExternalId,
                    CurrentStatus = GetTicketCurrentStatus(ticket),
                    Started = DateFriendlyText(ticket.Started),
                    Finished = DateFriendlyText(ticket.Finished),
                    Size = ticket.Size == 0 ? "?" : ticket.Size.ToString(),
                    IsCompleted = ticket.Finished > DateTime.MinValue
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
    }

    public class TicketViewModel
    {
        public ActivityBreakdown ActivityBreakdown { get; set; }

        public string Title { get; set; }

        public string ExternalId { get; set; }

        public string CurrentStatus { get; set; }

        public string Started { get; set; }

        public string Finished { get; set; }

        public string Size { get; set; }

        public bool IsCompleted { get; set; }
    }
}

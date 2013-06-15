using System;
using System.Collections.Generic;
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

            var assignedUsers = ticket.AssignedUsers.Select(u => new TicketContributor
                {
                    Name = GetFirstName(u.Name), Email = u.Email.Address
                }).ToList();

            return View(new TicketViewModel
                {
                    ActivityBreakdown = activityBreakdown,
                    Title = ticket.Title,
                    ExternalId = ticket.ExternalId,
                    CurrentStatus = GetTicketCurrentStatus(ticket),
                    Started = DateFriendlyText(ticket.Started),
                    Finished = DateFriendlyText(ticket.Finished),
                    Size = ticket.Size == 0 ? "?" : ticket.Size.ToString(),
                    IsCompleted = ticket.Finished > DateTime.MinValue,
                    Contributors = BuildTicketContributors(ticket),
                    CycleTime = ticket.CycleTime.Days,
                    AssignedUsers = assignedUsers
                });
        }

        private static string GetFirstName(string name)
        {
            return !name.Contains(" ") ? name : name.Split(' ')[0];
        }

        private static IEnumerable<TicketContributor> BuildTicketContributors(Ticket ticket)
        {
            var releventActivities = ticket.Activities.Where(a => a.AssignedUser != TicketAssignedUser.UnAssigned && (a.Title.ToUpper() == "DEV WIP" || a.Title.ToUpper() == "TEST WIP"));

            var activitiesGroupedByUserAndActivity = releventActivities.GroupBy(a => new
                TicketContributorGroupKey {
                    Activity = a.Title, 
                    AssignedUser = a.AssignedUser
                }, a => a, new TicketContributorGroupKeyComparer());

            var contributors = activitiesGroupedByUserAndActivity.Select(a =>
                {
                    var role = GetRole(a.Key.Activity);

                    return new TicketContributor
                        {
                            Name = a.Key.AssignedUser.Name, Email = a.Key.AssignedUser.Email.Address, Role = role
                        };
                });

            return contributors;
        }

        private static string GetRole(string activity)
        {
            return activity.Equals("dev wip", StringComparison.InvariantCultureIgnoreCase) ? "Developer" : "Tester";
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

    public class TicketContributorGroupKey
    {
        public TicketAssignedUser AssignedUser { get; set; }

        public string Activity { get; set; }
    }

    public class TicketContributorGroupKeyComparer : IEqualityComparer<TicketContributorGroupKey>
    {
        public bool Equals(TicketContributorGroupKey x, TicketContributorGroupKey y)
        {
            var activitiesMatch = x.Activity.Equals(y.Activity, StringComparison.InvariantCultureIgnoreCase);
            var usersMatch = x.AssignedUser.Id == y.AssignedUser.Id;

            return activitiesMatch && usersMatch;
        }

        public int GetHashCode(TicketContributorGroupKey key)
        {
            return key.Activity.GetHashCode() + key.AssignedUser.Id.GetHashCode();
        }
    }

    public class TicketContributor
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int DurationInHours { get; set; }

        public string Role { get; set; }
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

        public IEnumerable<TicketContributor> Contributors { get; set; }

        public TicketContributor AssignedTo { get; set; }

        public int CycleTime { get; set; }

        public IEnumerable<TicketContributor> AssignedUsers { get; set; }
    }
}

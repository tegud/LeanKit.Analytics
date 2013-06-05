using System;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Views.NewRelease
{
    public class NewReleaseController : Controller
    {
        [HttpPost]
        public RedirectResult Index(NewReleaseViewModel release)
        {
            const string connectionString = @"Data Source=.\Express2008;Initial Catalog=LeanKitSync;Persist Security Info=True;User ID=carduser;Password=password;MultipleActiveResultSets=True";

            var plannedDate = release.PlannedDate;
            var splitTime = release.PlannedTime.Split(':');
            var hours = int.Parse(splitTime[0]);
            var minutes = int.Parse(splitTime[1]);
            plannedDate = plannedDate.AddHours(hours);
            plannedDate = plannedDate.AddMinutes(minutes);

            new ReleaseRepository(connectionString).Create(new ReleaseRecord
                {
                    PlannedDate = plannedDate,
                    IncludedTickets = release.SelectedTickets.Split(',').Select(ticketId => new IncludedTicketRecord
                        {
                            CardId = int.Parse((ticketId))
                        }).ToList()
                });

            return new RedirectResult("/");
        }
    }

    public class NewReleaseViewModel
    {
        public DateTime PlannedDate { get; set; }

        public string PlannedTime { get; set; }

        public string SelectedTickets { get; set; }
    }
}

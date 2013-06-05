using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class NewReleaseController : Controller
    {
        [HttpPost]
        public RedirectResult Index(NewReleaseViewModel release)
        {
            var connectionString = MvcApplication.ConnectionString;

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
}

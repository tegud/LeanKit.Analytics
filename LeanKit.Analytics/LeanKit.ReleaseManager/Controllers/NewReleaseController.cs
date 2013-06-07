using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class NewReleaseController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public NewReleaseController(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        [HttpPost]
        public RedirectResult Index(NewReleaseViewModel release)
        {
            var plannedDate = release.PlannedDate;
            var splitTime = release.PlannedTime.Split(':');
            var hours = int.Parse(splitTime[0]);
            var minutes = int.Parse(splitTime[1]);
            plannedDate = plannedDate.AddHours(hours);
            plannedDate = plannedDate.AddMinutes(minutes);

            _releaseRepository.Create(new ReleaseRecord
                {
                    PlannedDate = plannedDate,
                    IncludedTickets = release.SelectedTickets.Split(',').Select(ticketId => new IncludedTicketRecord
                        {
                            CardId = int.Parse((ticketId))
                        }).ToList()
                });

            return new RedirectResult("/UpcomingReleases");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleasesController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public ReleasesController(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ActionResult Index()
        {
            var allReleases = _releaseRepository.GetAllReleases();

            return View(new ListOfReleasesViewModel
                {
                    Releases = allReleases.Select(r => new ReleaseListItemViewModel
                        {
                            Id = r.Id,
                            FormattedPlannedDate = r.PlannedDate.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                            FormattedActualStartedDate = r.StartedAt.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                            FormattedActualEndDate = r.CompletedAt.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                            NumberOfIncludedTickets = r.IncludedTickets.Count()
                        })
                });
        }
    }

    public class ListOfReleasesViewModel
    {
        public IEnumerable<ReleaseListItemViewModel> Releases { get; set; }
    }

    public class ReleaseListItemViewModel
    {
        public int Id { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }

        public string FormattedPlannedDate { get; set; }

        public string FormattedActualStartedDate { get; set; }

        public string FormattedActualEndDate { get; set; }

        public int NumberOfIncludedTickets { get; set; }

    }
}

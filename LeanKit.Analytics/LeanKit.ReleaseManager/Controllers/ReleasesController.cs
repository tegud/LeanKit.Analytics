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
                    Releases = allReleases.Select(r =>
                        {
                            var totalMinutes = (r.CompletedAt - r.StartedAt).TotalMinutes;
                            string formattedTotalMinutes = string.Empty;

                            if(totalMinutes > 0)
                            {
                                formattedTotalMinutes = string.Format("{0} min{1}", 
                                    totalMinutes,
                                    totalMinutes > 1 ? "s" : string.Empty);
                            }

                            return new ReleaseListItemViewModel
                                                               {
                                                                   Id = r.Id,
                                                                   SvnRevision = r.SvnRevision,
                                                                   ServiceNowId = r.ServiceNowId,
                                                                   FormattedPlannedDate = r.PlannedDate.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                                                                   FormattedActualStartedDate = r.StartedAt.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                                                                   FormattedActualEndDate = r.CompletedAt.ToFriendlyText("dd MMM yyyy", "\" at \" HH:mm"),
                                                                   FormattedActualDuration = formattedTotalMinutes,
                                                                   NumberOfIncludedTickets = r.IncludedTickets.Count(t => t != null)
                                                               };
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

        public string FormattedActualDuration { get; set; }
    }
}

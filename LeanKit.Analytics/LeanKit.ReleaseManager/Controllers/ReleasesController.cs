using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleasesController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IMakeTimePeriodViewModels _timePeriodViewModelFactory;
        private readonly IMakeCycleTimeQueries _queryFactory;

        public ReleasesController(IGetReleasesFromTheDatabase releaseRepository,
            IMakeTimePeriodViewModels timePeriodViewModelFactory,
            IMakeCycleTimeQueries queryFactory)
        {
            _releaseRepository = releaseRepository;
            _timePeriodViewModelFactory = timePeriodViewModelFactory;
            _queryFactory = queryFactory;
        }

        public ActionResult Index(string timePeriod)
        {
            var query = _queryFactory.Build(timePeriod);

            var allReleases = _releaseRepository.GetAllReleases(query);

            return View(new ListOfReleasesViewModel
                {
                    TimePeriods = _timePeriodViewModelFactory.Build(query.Period),
                    Releases = allReleases.Select(r =>
                        {
                            var totalMinutes = (r.CompletedAt - r.StartedAt).TotalMinutes;
                            var formattedTotalMinutes = string.Empty;

                            if(totalMinutes > 0)
                            {
                                formattedTotalMinutes = string.Format("{0:0.00} min{1}", 
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
                                                                   NumberOfIncludedTickets = r.IncludedTickets.Count(t => t != null),
                                                                   WasRolledBack = r.RollbackDate > DateTime.MinValue,
                                                                   RollbackReason = r.RollbackReason
                                                               };
                        })
                });
        }
    }

    public class ListOfReleasesViewModel
    {
        public IEnumerable<ReleaseListItemViewModel> Releases { get; set; }

        public CycleTimePeriodViewModel TimePeriods { get; set; }
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

        public bool WasRolledBack { get; set; }

        public string RollbackReason { get; set; }
    }
}

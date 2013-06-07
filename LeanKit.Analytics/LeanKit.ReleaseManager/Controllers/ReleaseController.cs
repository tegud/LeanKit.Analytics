using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        const string FRIENDLY_TEXT_DATE = "dddd dd MMM yyyy";
        const string FRIENDLY_TEXT_TIME = " \"at\" HH:mm";

        public ReleaseController(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ActionResult Index(int id)
        {
            var releaseRecord = _releaseRepository.GetRelease(id);

            var releaseActualTime = GetReleaseActualTime(releaseRecord);

            var releasePlannedTime = new ReleasePlannedTime
                {
                    StartFriendlyText = releaseRecord.PlannedDate.ToFriendlyText(FRIENDLY_TEXT_DATE, FRIENDLY_TEXT_TIME), 
                    Duration = GetPlannedDurationText(releaseRecord)
                };

            var hasStarted = releaseRecord.StartedAt > DateTime.MinValue;
            var hasCompleted = releaseRecord.CompletedAt > DateTime.MinValue;
            var releaseStatusViewModel = new ReleaseStatusViewModel
                {
                    HasStarted = hasStarted,
                    HasCompleted = hasCompleted,
                    Text = hasStarted ? hasCompleted ? "Completed" : "In Progress" : "Awaiting Approval"
                };

            var releaseDetailIncludedTicketViewModels = releaseRecord.IncludedTickets.Any() ? releaseRecord.IncludedTickets.Select(rr => new ReleaseDetailIncludedTicketViewModel
                {
                    ExternalId = rr.ExternalId, Title = rr.Title, Size = rr.Size > 0 ? rr.Size.ToString() : "?"
                }) : new List<ReleaseDetailIncludedTicketViewModel>(0);
            var releaseViewModel = new ReleaseDetailViewModel
                {
                    Id = id,
                    PlannedTime = releasePlannedTime,
                    ActualTime = releaseActualTime,
                    Status = releaseStatusViewModel,
                    IncludedTickets = releaseDetailIncludedTicketViewModels
                };
            return View("Index", releaseViewModel);
        }

        private static ReleaseActualTime GetReleaseActualTime(ReleaseRecord releaseRecord)
        {
            if (releaseRecord.StartedAt == DateTime.MinValue)
            {
                return new ReleaseActualTime();
            }

            if (releaseRecord.CompletedAt == DateTime.MinValue)
            {
                return new ReleaseActualTime
                    {
                        StartedFriendlyText = releaseRecord.StartedAt.ToFriendlyText(FRIENDLY_TEXT_DATE, FRIENDLY_TEXT_TIME)
                    };
            }

            return new ReleaseActualTime
                {
                    StartedFriendlyText = releaseRecord.StartedAt.ToFriendlyText(FRIENDLY_TEXT_DATE, FRIENDLY_TEXT_TIME),
                    CompletedFriendlyText = releaseRecord.CompletedAt.ToFriendlyText(FRIENDLY_TEXT_DATE, FRIENDLY_TEXT_TIME),
                    Duration = (releaseRecord.CompletedAt - releaseRecord.StartedAt).TotalMinutes
                };
        }

        private static string GetPlannedDurationText(ReleaseRecord releaseRecord)
        {
            return releaseRecord.PlannedDuration > 0 ? string.Format(" ({0} min{1})", releaseRecord.PlannedDate,
                                                                     releaseRecord.PlannedDuration > 1 ? "s" : "") : "";
        }
    }

    public class ReleaseDetailViewModel
    {
        public int Id { get; set; }

        public ReleasePlannedTime PlannedTime { get; set; }

        public ReleaseActualTime ActualTime { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }

        public ReleaseStatusViewModel Status { get; set; }

        public IEnumerable<ReleaseDetailIncludedTicketViewModel> IncludedTickets { get; set; }
    }

    public class ReleaseDetailIncludedTicketViewModel
    {
        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string Size { get; set; }

        public IEnumerable<ReleaseDetailTicketApproval> Approvals { get; set; }
    }

    public class ReleaseDetailTicketApproval
    {
        public string Name { get; set; }

        public bool Approved { get; set; }
    }

    public class ReleaseStatusViewModel
    {
        public bool HasStarted { get; set; }

        public bool HasCompleted { get; set; }

        public string Text { get; set; }

        public string CssClass { get; set; }
    }

    public class ReleasePlannedTime
    {
        public string StartFriendlyText { get; set; }

        public string Duration { get; set; }
    }

    public class ReleaseActualTime
    {
        public string StartedFriendlyText { get; set; }

        public string CompletedFriendlyText { get; set; }

        public double Duration { get; set; }
    }
}

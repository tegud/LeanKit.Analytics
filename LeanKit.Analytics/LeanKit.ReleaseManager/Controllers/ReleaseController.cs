using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
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

        public ContentResult SetStarted(int id, DateTime started)
        {
            _releaseRepository.SetStartedDate(id, started);

            return Content(string.Empty);
        }

        public ContentResult SetCompleted(int id, DateTime completed)
        {
            _releaseRepository.SetCompletedDate(id, completed);

            return Content(string.Empty);
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
                    ExternalId = rr.ExternalId,
                    Title = rr.Title,
                    Size = rr.Size > 0 ? rr.Size.ToString() : "?"
                }) : new List<ReleaseDetailIncludedTicketViewModel>(0);
            var releaseViewModel = new ReleaseDetailViewModel
                {
                    Id = id,
                    PlannedTime = releasePlannedTime,
                    ActualTime = releaseActualTime,
                    Status = releaseStatusViewModel,
                    IncludedTickets = releaseDetailIncludedTicketViewModels,
                    SvnRevision = releaseRecord.SvnRevision,
                    ServiceNowId = releaseRecord.ServiceNowId
                };
            return View("Index", releaseViewModel);
        }

        private static ReleaseActualTime GetReleaseActualTime(ReleaseRecord releaseRecord)
        {
            if (releaseRecord.StartedAt == DateTime.MinValue)
            {
                return new ReleaseActualTime();
            }

            const string dateFieldFormat = "yyyy-MM-dd";
            const string timeFieldFormat = "HH:mm";

            if (releaseRecord.CompletedAt == DateTime.MinValue)
            {
                return new ReleaseActualTime
                    {
                        StartedAt = releaseRecord.StartedAt,
                        StartedAtDateFieldValue = releaseRecord.StartedAt.ToString(dateFieldFormat),
                        StartedAtTimeFieldValue = releaseRecord.StartedAt.ToString(timeFieldFormat),
                        StartedFriendlyText = releaseRecord.StartedAt.ToFriendlyText(FRIENDLY_TEXT_DATE, FRIENDLY_TEXT_TIME)
                    };
            }

            return new ReleaseActualTime
            {
                StartedAt = releaseRecord.StartedAt,
                StartedAtDateFieldValue = releaseRecord.StartedAt.ToString(dateFieldFormat),
                StartedAtTimeFieldValue = releaseRecord.StartedAt.ToString(timeFieldFormat),
                CompletedAtDateFieldValue = releaseRecord.CompletedAt.ToString(dateFieldFormat),
                CompletedAtTimeFieldValue = releaseRecord.CompletedAt.ToString(timeFieldFormat),
                CompletedAt = releaseRecord.CompletedAt,
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
}

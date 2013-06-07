using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public ReleaseController(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ActionResult Index(int id)
        {
            var releaseRecord = _releaseRepository.GetRelease(id);

            var releaseActualTime = new ReleaseActualTime
                {
                    StartedFriendlyText = "Today at 10:00", 
                    CompletedFriendlyText = "Today at 10:45", 
                    Duration = 45
                };

            var releasePlannedTime = new ReleasePlannedTime
                {
                    StartFriendlyText = releaseRecord.PlannedDate.ToFriendlyText("dddd dd MMM yyyy", " \"at\" HH:mm"), Duration = releaseRecord.PlannedDuration
                };

            var releaseViewModel = new ReleaseDetailViewModel
                {
                    Id = id,
                    PlannedTime = releasePlannedTime,
                    Completed = true,
                    ActualTime = releaseActualTime,
                    Status = new ReleaseStatusViewModel
                        {
                            Text = "Awaiting Approval"
                        }
                };
            return View("Index", releaseViewModel);
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

        public bool Completed { get; set; }
    }

    public class ReleaseStatusViewModel
    {
        public string Text { get; set; }

        public string CssClass { get; set; }
    }

    public class ReleasePlannedTime
    {
        public string StartFriendlyText { get; set; }

        public int Duration { get; set; }
    }

    public class ReleaseActualTime
    {
        public string StartedFriendlyText { get; set; }

        public string CompletedFriendlyText { get; set; }

        public int Duration { get; set; }
    }
}

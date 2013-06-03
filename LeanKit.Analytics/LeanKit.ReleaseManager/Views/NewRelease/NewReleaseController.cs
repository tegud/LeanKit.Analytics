using System;
using System.Web.Mvc;

namespace LeanKit.ReleaseManager.Views.NewRelease
{
    public class NewReleaseController : Controller
    {
        [HttpPost]
        public RedirectResult Index(NewReleaseViewModel release)
        {
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

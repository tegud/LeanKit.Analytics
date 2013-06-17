using System.Collections.Generic;
using System.Web.Mvc;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ProductOwnerDashboardController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProductOwnerDashboardViewModel
                {
                    ReleaseCount = 4,
                    TicketsCompletedCount = 9,
                    ComplexityPointsReleased = 7,
                    AverageCycleTime = 3,
                    SelectedTimePeriodFriendlyName = "this week"
                });
        }
    }

    public class ProductOwnerDashboardViewModel
    {
        public int ReleaseCount { get; set; }

        public int TicketsCompletedCount { get; set; }

        public int ComplexityPointsReleased { get; set; }

        public int AverageCycleTime { get; set; }

        public string SelectedTimePeriodFriendlyName { get; set; }

        public IEnumerable<ProductOwnerDashboardReleaseViewModel> Releases { get; set; }

        public IEnumerable<ProductOwnerDashboardBlockagesViewModel> Blockages { get; set; }
    }

    public class ProductOwnerDashboardReleaseViewModel
    {
        
    }

    public class ProductOwnerDashboardBlockagesViewModel
    {
         
    }
}

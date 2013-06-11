using System.Web.Mvc;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class CycleTimeController : Controller
    {
        private readonly IBuildCycleTimeViewModels _cycleTimeViewModelFactory;

        public CycleTimeController(IBuildCycleTimeViewModels cycleTimeViewModelFactory)
        {
            _cycleTimeViewModelFactory = cycleTimeViewModelFactory;
        }

        public ViewResult Index()
        {
            return View(_cycleTimeViewModelFactory.Build());
        }
    }
}

using System.Web.Mvc;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class CycleTimeController : Controller
    {
        private readonly IBuildCycleTimeViewModels _cycleTimeViewModelFactory;
        private readonly IMakeCycleTimeQueries _queryFactory;

        public CycleTimeController(IBuildCycleTimeViewModels cycleTimeViewModelFactory, IMakeCycleTimeQueries queryFactory)
        {
            _cycleTimeViewModelFactory = cycleTimeViewModelFactory;
            _queryFactory = queryFactory;
        }

        public ViewResult Index(string timePeriod)
        {
            var query = _queryFactory.Build(timePeriod);

            return View(_cycleTimeViewModelFactory.Build(query));
        }
    }
}

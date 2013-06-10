using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class NewReleaseController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IBuildNewReleaseRecords _createReleaseReleaseRecordFactory;

        public NewReleaseController(IGetReleasesFromTheDatabase releaseRepository,
            IBuildNewReleaseRecords createReleaseReleaseRecordFactory)
        {
            _releaseRepository = releaseRepository;
            _createReleaseReleaseRecordFactory = createReleaseReleaseRecordFactory;
        }

        [HttpPost]
        public RedirectResult Index(ReleaseInputModel release)
        {
            var releaseRecord = _createReleaseReleaseRecordFactory.Build(release);

            var releaseId = _releaseRepository.Create(releaseRecord);

            return new RedirectResult("/Release/" + releaseId);
        }
    }
}

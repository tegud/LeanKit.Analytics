using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Controllers
{
    public class UpdateExistingReleaseController : Controller
    {
        private readonly IBuildNewReleaseRecords _createReleaseReleaseRecordFactory;
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public UpdateExistingReleaseController(IGetReleasesFromTheDatabase releaseRepository,
            IBuildNewReleaseRecords createReleaseReleaseRecordFactory)
        {
            _releaseRepository = releaseRepository;
            _createReleaseReleaseRecordFactory = createReleaseReleaseRecordFactory;
        }

        [HttpPost]
        public RedirectResult Index(ReleaseInputModel release)
        {
            var releaseRecord = _createReleaseReleaseRecordFactory.Build(release);

            _releaseRepository.Update(releaseRecord);

            return new RedirectResult("/Release/" + releaseRecord.Id);
        }
    }
}

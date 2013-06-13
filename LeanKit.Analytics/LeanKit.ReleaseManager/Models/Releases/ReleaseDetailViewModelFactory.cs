using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseDetailViewModelFactory
    {
        private IGetReleasesFromTheDatabase _releaseRepository;

        public ReleaseDetailViewModelFactory(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ReleaseDetailViewModel Build(int id)
        {
            var release = _releaseRepository.GetRelease(id);



            return new ReleaseDetailViewModel
                {
                    Id = release.Id,
                    SvnRevision = release.SvnRevision
                };
        }
    }
}
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildNewReleaseRecords
    {
        ReleaseRecord Build(ReleaseInputModel release);
    }
}
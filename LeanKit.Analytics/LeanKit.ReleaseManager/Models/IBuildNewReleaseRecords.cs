using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildNewReleaseRecords
    {
        ReleaseRecord Build(NewReleaseViewModel release);
    }
}
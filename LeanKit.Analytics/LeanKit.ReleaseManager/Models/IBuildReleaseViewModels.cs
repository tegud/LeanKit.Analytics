using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildReleaseViewModels
    {
        ReleaseViewModel BuildReleaseViewModel(ReleaseRecord releaseRecord, IRotateThroughASetOfColours colourPalette);
    }
}
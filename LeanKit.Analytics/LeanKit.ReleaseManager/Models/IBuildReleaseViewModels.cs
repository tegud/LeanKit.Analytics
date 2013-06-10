using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildReleaseViewModels
    {
        ReleaseViewModel BuildReleaseViewModel(ReleaseRecord releaseRecord, IRotateThroughASetOfColours colourPalette);
    }
}
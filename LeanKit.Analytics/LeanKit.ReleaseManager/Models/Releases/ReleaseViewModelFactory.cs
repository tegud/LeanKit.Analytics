using System.Linq;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseViewModelFactory : IBuildReleaseViewModels
    {
        public ReleaseViewModel BuildReleaseViewModel(ReleaseRecord releaseRecord, IRotateThroughASetOfColours colourPalette)
        {
            return new ReleaseViewModel
                {
                    Id = releaseRecord.Id,
                    PlannedDate = releaseRecord.PlannedDate,
                    DateFriendlyText = releaseRecord.PlannedDate.ToFriendlyText("dd MMM yyyy", " \"at\" HH:mm"),
                    Color = colourPalette.Next(),
                    IncludedTickets = releaseRecord.IncludedTickets.Count()
                };
        }
    }
}
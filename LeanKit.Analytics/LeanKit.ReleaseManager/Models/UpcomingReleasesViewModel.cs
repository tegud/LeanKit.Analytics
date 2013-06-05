using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class UpcomingReleasesViewModel
    {
        public IEnumerable<ReleaseViewModel> Releases { get; set; }

        public IEnumerable<LaneColumn> Lanes { get; set; }

        public string NextReleaseColor { get; set; }

        public CreateReleaseModel CreateReleaseModel { get; set; }
    }
}
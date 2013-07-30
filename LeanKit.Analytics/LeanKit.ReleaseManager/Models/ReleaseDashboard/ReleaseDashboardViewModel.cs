using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public class ReleaseDashboardViewModel
    {
        public ServerStatusViewModel ServerStatus { get; set; }

        public ReleaseRecord LastRelease { get; set; }
    }
}
using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public class ServerStatusViewModel
    {
        public IEnumerable<DeploymentGroupViewModel> DeploymentGroups { get; set; }
    }
}
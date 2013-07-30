using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public class DeploymentGroupViewModel
    {
        public string Name { get; set; }

        public IEnumerable<ServerViewModel> Servers { get; set; }
    }
}
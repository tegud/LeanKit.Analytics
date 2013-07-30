using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public class ReleaseDashboardViewModelFactory
    {
        public ReleaseDashboardViewModel Build()
        {
            var webServers = new List<ServerViewModel>();
            var sslServers = new List<ServerViewModel>();
            var auServers = new List<ServerViewModel>();

            for (var x = 1; x < 20; x++)
            {
                webServers.Add(new ServerViewModel { Host = string.Format("telweb{0:000}P", x) });
            }

            for (var x = 107; x < 110; x++)
            {
                sslServers.Add(new ServerViewModel { Host = string.Format("telweb{0:000}P", x) });
            }

            for (var x = 320; x < 323; x++)
            {
                auServers.Add(new ServerViewModel { Host = string.Format("telweb{0:000}P", x) });
            }

            return new ReleaseDashboardViewModel
                {
                    ServerStatus = new ServerStatusViewModel
                        {
                            DeploymentGroups = new List<DeploymentGroupViewModel>
                                {
                                    new DeploymentGroupViewModel
                                        {
                                            Name = "Web",
                                            Servers = webServers
                                        },
                                    new DeploymentGroupViewModel
                                        {
                                            Name = "SSL",
                                            Servers = sslServers
                                        },
                                    new DeploymentGroupViewModel
                                        {
                                            Name = "AU",
                                            Servers = auServers
                                        }
                                }
                        }
                };
        }
    }
}
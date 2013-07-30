using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Models.ReleaseDashboard
{
    public interface IReleaseDashboardViewModelFactory
    {
        ReleaseDashboardViewModel Build();
    }

    public class ReleaseDashboardViewModelFactory : IReleaseDashboardViewModelFactory
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public ReleaseDashboardViewModelFactory(IGetReleasesFromTheDatabase releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ReleaseDashboardViewModel Build()
        {
            var allReleases = _releaseRepository.GetAllReleases(new CycleTimeQuery());
            
            var webServers = new List<ServerViewModel>();
            var sslServers = new List<ServerViewModel>();
            var auServers = new List<ServerViewModel>();

            for (var x = 1; x < 20; x++)
            {
                webServers.Add(new ServerViewModel { Id = x });
            }

            for (var x = 107; x < 110; x++)
            {
                sslServers.Add(new ServerViewModel { Id = x });
            }

            for (var x = 320; x < 323; x++)
            {
                auServers.Add(new ServerViewModel { Id = x });
            }

            return new ReleaseDashboardViewModel
                {
                    LastRelease = allReleases.FirstOrDefault(r => r.StartedAt > DateTime.MinValue),
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
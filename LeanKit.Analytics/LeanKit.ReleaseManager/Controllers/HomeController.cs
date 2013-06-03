using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.APIClient.API;
using LeanKit.Data.SQL;

namespace LeanKit.ReleaseManager.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            const string username = "steve.elliot@laterooms.com";
            const string password = "10Four12";
            const string account = "lrtest";
            const string boardId = "32482312";
            const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=LeanKitSync;Persist Security Info=True;User ID=carduser;Password=password;MultipleActiveResultSets=True";

            var apiCaller = new ApiCaller
            {
                Account = account,
                BoardId = boardId,
                Credentials = new ApiCredentials
                {
                    Username = username,
                    Password = password
                }
            };

            var colors = new[]
                {
                    "#D15300",
                    "#83BF00",
                    "#FFF268",
                    "#9682FF",
                    "#FF9999"
                };

            var releases = new ReleaseRepository(connectionString).GetUpcomingReleases().Select((r, i) =>
                {
                    string friendlyText = null;
                    var todaysDate = DateTime.Now.Date;
                    var plannedReleaseDate = r.PlannedDate.Date;

                    if (plannedReleaseDate == todaysDate)
                    {
                        friendlyText = string.Format("Today at {0:hh:mm}", r.PlannedDate);
                    }
                    else if (plannedReleaseDate == todaysDate.AddDays(1))
                    {
                        friendlyText = string.Format("Tomorrow at {0:hh:mm}", r.PlannedDate);
                    }

                    return new ReleaseViewModel
                        {
                            Id = r.Id,
                            PlannedDate = r.PlannedDate,
                            DateFriendlyText = friendlyText ?? r.PlannedDate.ToString("dd MMM yyyy hh:mm"),
                            Color = colors[i % colors.Length]
                        };
                });

            var board = apiCaller.GetBoard();

            var laneColumns = board.Lanes.Where(l => l.Title != "Live").Select(l => new LaneColumn
                {
                    Title = l.Title,
                    Tickets = l.Cards.Select(c =>
                        {
                            var allMachineTags = c.Tags.Split(',').Where(t => t.StartsWith("#"));
                            var releaseId = allMachineTags.Where(t => t.StartsWith("#Rel")).Select(t => t.Substring(4)).FirstOrDefault();
                            int parsedReleaseId;
                            string color = null;

                            if (!string.IsNullOrWhiteSpace(releaseId) && int.TryParse(releaseId, out parsedReleaseId))
                            {
                                var matchingRelease = releases.FirstOrDefault(r => r.Id == parsedReleaseId);

                                if(matchingRelease != null)
                                {
                                    color = matchingRelease.Color;
                                    matchingRelease.TicketCount = matchingRelease.TicketCount + 1;
                                }
                            }

                            return new ReleaseTicket
                                {
                                    Id = c.Id,
                                    ExternalId = c.ExternalCardID,
                                    Title = c.Title,
                                    Color = color
                                };
                        })
                });

            var upcomingReleasesViewModel = new UpcomingReleasesViewModel
                {
                    Releases = releases,
                    Lanes = laneColumns,
                    NextReleaseColor = colors[(releases.Count() + 1) % colors.Length]
                };
            return View(upcomingReleasesViewModel);
        }
    }

    public class UpcomingReleasesViewModel
    {
        public IEnumerable<ReleaseViewModel> Releases { get; set; }

        public IEnumerable<LaneColumn> Lanes { get; set; }

        public string NextReleaseColor { get; set; }
    }

    public class ReleaseViewModel
    {
        public int Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public IEnumerable<ReleaseTicket> Tickets { get; set; }

        public string DateFriendlyText { get; set; }

        public int TicketCount { get; set; }

        public string Color { get; set; }
    }

    public class LaneColumn
    {
        public string Title { get; set; }

        public IEnumerable<ReleaseTicket> Tickets { get; set; }
    }

    public class ReleaseTicket
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string Color { get; set; }
    }

}

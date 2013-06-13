using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.Releases;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class EditReleaseController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IGetActivitiesFromTheDatabase _activityRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRotateThroughASetOfColours _colourPalette;

        public EditReleaseController(IGetReleasesFromTheDatabase releaseRepository,
            IGetActivitiesFromTheDatabase activityRepository,
            ITicketRepository ticketRepository,
            IRotateThroughASetOfColours colourPalette)
        {
            _releaseRepository = releaseRepository;
            _activityRepository = activityRepository;
            _ticketRepository = ticketRepository;
            _colourPalette = colourPalette;
        }

        public ViewResult Index(int id)
        {
            var allTickets = _ticketRepository.GetAll().Tickets;
            var releaseRecord = _releaseRepository.GetRelease(id);

            var lanes = _activityRepository.GetLanes().Where(l => l.Title != "Live");
            var releaseColour = _colourPalette.Next();

            var laneColumns = lanes.Select(l => new LaneColumn
                {
                    Title = l.Title,
                    Tickets = allTickets.Where(t => t.CurrentActivity.Title == l.Title).Select(t =>
                        {
                            var inRelease = releaseRecord.IncludedTickets.Any(it => it.CardId == t.Id);

                            var releaseTicket = new ReleaseTicket
                            {
                                Id = t.Id,
                                Title = t.Title,
                                ExternalId = t.ExternalId
                            };

                            if (inRelease)
                            {
                                releaseTicket.Color = releaseColour;
                            }

                            return releaseTicket;
                        }).ToArray()
                }).ToArray();

            var upcomingReleasesViewModel = new EditReleaseViewModel
                {
                    Id = id,
                    ReleaseColour = releaseColour,
                    Lanes = laneColumns,
                    SvnRevision = releaseRecord.SvnRevision,
                    ServiceNowId = releaseRecord.ServiceNowId,
                    FormattedStartDate = releaseRecord.PlannedDate.ToString("yyyy-MM-dd"),
                    FormattedStartTime = releaseRecord.PlannedDate.ToString("HH:mm"),
                    IncludedTickets = string.Join(",", releaseRecord.IncludedTickets.Select(t => t.CardId))
                };

            return View(upcomingReleasesViewModel);
        }
    }

    public class EditReleaseViewModel
    {
        public IEnumerable<LaneColumn> Lanes { get; set; }

        public string ReleaseColour { get; set; }

        public int Id { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }

        public string FormattedStartDate { get; set; }

        public string FormattedStartTime { get; set; }

        public string IncludedTickets { get; set; }
    }
}
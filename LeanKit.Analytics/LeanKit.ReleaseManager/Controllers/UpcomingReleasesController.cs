﻿using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class UpcomingReleasesController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IGetActivitiesFromTheDatabase _activityRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMakeListsOfDateOptions _dateOptionsFactory;
        private readonly IRotateThroughASetOfColours _colourPalette;

        public UpcomingReleasesController(IGetReleasesFromTheDatabase releaseRepository,
            IGetActivitiesFromTheDatabase activityRepository,
            ITicketRepository ticketRepository, 
            IMakeListsOfDateOptions dateOptionsFactory,
            IRotateThroughASetOfColours colourPalette)
        {
            _releaseRepository = releaseRepository;
            _activityRepository = activityRepository;
            _ticketRepository = ticketRepository;
            _dateOptionsFactory = dateOptionsFactory;
            _colourPalette = colourPalette;
        }

        public ViewResult Index()
        {
            var allTickets = _ticketRepository.GetAll().Tickets;
            var releaseRecords = _releaseRepository.GetUpcomingReleases().ToArray();

            var lanes = _activityRepository.GetLanes().Where(l => l.Title != "Live");

            var releases = releaseRecords.Select((r, i) => new ReleaseViewModel
                {
                    Id = r.Id,
                    PlannedDate = r.PlannedDate,
                    DateFriendlyText = r.PlannedDate.ToFriendlyText("dd MMM yyyy", " \"at\" HH:mm"),
                    Color = _colourPalette .Next()
                }).ToArray();

            var laneColumns = lanes.Select(l => new LaneColumn
                {
                    Title = l.Title,
                    Tickets = allTickets.Where(t => t.CurrentActivity.Title == l.Title).Select(t =>
                        {
                            var matchingReleaseRecord = releaseRecords.FirstOrDefault(rr => rr.IncludedTickets.Select(it => it.CardId).Contains(t.Id));

                            var releaseTicket = new ReleaseTicket
                            {
                                Id = t.Id,
                                Title = t.Title,
                                ExternalId = t.ExternalId
                            };

                            if (matchingReleaseRecord != null)
                            {
                                var matchingRelease = releases.First(r => r.Id == matchingReleaseRecord.Id);
                                releaseTicket.Color = matchingRelease.Color;
                                matchingRelease.Tickets.Add(releaseTicket);
                            }

                            return releaseTicket;
                        }).ToArray()
                }).ToArray();

            var upcomingReleasesViewModel = new UpcomingReleasesViewModel
                {
                    Releases = releases,
                    Lanes = laneColumns,
                    NextReleaseColor = _colourPalette.Next(),
                    CreateReleaseModel = new CreateReleaseModel
                        {
                            DateOptions = _dateOptionsFactory.BuildDateOptions(5)
                        }
                };

            return View(upcomingReleasesViewModel);
        }
    }
}

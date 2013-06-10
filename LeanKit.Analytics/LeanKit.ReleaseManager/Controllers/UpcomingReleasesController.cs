using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;

namespace LeanKit.ReleaseManager.Controllers
{
    public class UpcomingReleasesController : Controller
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IGetActivitiesFromTheDatabase _activityRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMakeListsOfDateOptions _dateOptionsFactory;
        private readonly IRotateThroughASetOfColours _colourPalette;
        private readonly ReleaseViewModelFactory _releaseViewModelFactory;

        public UpcomingReleasesController(IGetReleasesFromTheDatabase releaseRepository,
            IGetActivitiesFromTheDatabase activityRepository,
            ITicketRepository ticketRepository, 
            IMakeListsOfDateOptions dateOptionsFactory,
            IRotateThroughASetOfColours colourPalette, 
            ReleaseViewModelFactory releaseViewModelFactory)
        {
            _releaseRepository = releaseRepository;
            _activityRepository = activityRepository;
            _ticketRepository = ticketRepository;
            _dateOptionsFactory = dateOptionsFactory;
            _colourPalette = colourPalette;
            _releaseViewModelFactory = releaseViewModelFactory;
        }

        public ViewResult Index()
        {
            var allTickets = _ticketRepository.GetAll().Tickets;

            var lanes = _activityRepository.GetLanes().Where(l => l.Title != "Live");

            var releaseRecords = _releaseRepository.GetUpcomingReleases().ToArray();
            var releases = releaseRecords.Select(releaseRecord => _releaseViewModelFactory.BuildReleaseViewModel(releaseRecord, _colourPalette)).ToArray();
            var laneColumns = lanes.Select(l => BuildLane(l, allTickets, releaseRecords, releases)).ToArray();

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


        private static LaneColumn BuildLane(Activity activity, 
            IEnumerable<Ticket> allTickets, 
            IEnumerable<ReleaseRecord> releaseRecords, 
            IEnumerable<ReleaseViewModel> releases)
        {
            return new LaneColumn
                {
                    Title = activity.Title,
                    Tickets = allTickets.Where(t => t.CurrentActivity.Title == activity.Title).Select(t =>
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
                            }

                            return releaseTicket;
                        }).ToArray()
                };
        }
    }
}

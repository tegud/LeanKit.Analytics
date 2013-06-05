using System;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var connectionString = MvcApplication.ConnectionString;

            var workDurationFactory = new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
            {
                Start = 9,
                End = 17
            });
            var activityIsInProgressSpecification = new ActivityIsInProgressSpecification();
            IActivitySpecification activityIsLiveSpecification = new ActivityIsLiveSpecification();

            var dateTimeWrapper = new DateTimeWrapper();
            var ticketCycleTimeDurationFactory = new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper);
            var ticketStartDateFactory = new TicketStartDateFactory(activityIsInProgressSpecification);
            var ticketFinishDateFactory = new TicketFinishDateFactory(activityIsLiveSpecification);
            var sqlTicketActivityFactory = new TicketActivityFactory(workDurationFactory);
            var sqlTicketCurrentActivityFactory = new CurrentActivityFactory();
            var sqlTicketFactory = new TicketFactory(ticketStartDateFactory, ticketFinishDateFactory, sqlTicketActivityFactory, ticketCycleTimeDurationFactory, sqlTicketCurrentActivityFactory);
            var dateOptionsFactory = new DateOptionsFactory();

            var releaseRepository = new ReleaseRepository(connectionString);
            var activityRepository = new ActivityRepository(connectionString);
            var ticketRepository = new TicketsRepository(connectionString, sqlTicketFactory);

            var allTickets = ticketRepository.GetAll().Tickets;
            var releaseRecords = releaseRepository.GetUpcomingReleases().ToArray();

            var colourPalette = new ColourPalette(new[]
                {
                    "#D15300",
                    "#83BF00",
                    "#FFF268",
                    "#9682FF",
                    "#FF9999"
                });

            var lanes = activityRepository.GetLanes().Where(l => l.Title != "Live");

            var releases = releaseRecords.Select((r, i) => new ReleaseViewModel
                {
                    Id = r.Id,
                    PlannedDate = r.PlannedDate,
                    DateFriendlyText = r.PlannedDate.ToFriendlyText("dd MMM yyyy", " \"at\" HH:mm"),
                    Color = colourPalette .Next()
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
                    NextReleaseColor = colourPalette.Next(),
                    CreateReleaseModel = new CreateReleaseModel
                        {
                            DateOptions = dateOptionsFactory.BuildDateOptions(5)
                        }
                };

            return View(upcomingReleasesViewModel);
        }
    }
}

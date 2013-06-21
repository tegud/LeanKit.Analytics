using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ProductOwnerDashboardController : Controller
    {
        private readonly IBuildListOfCycleTimeItems _listOfCycleTimeItemsFactory;
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly IMakeCycleTimeQueries _queryFactory;
        private readonly IMakeTimePeriodViewModels _timePeriodViewModelFactory;
        private readonly IGetReleasesFromTheDatabase _releaseRepository;

        public ProductOwnerDashboardController(IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory, 
            IGetReleasedTicketsFromTheDatabase ticketRepository,
            IMakeCycleTimeQueries queryFactory, 
            IMakeTimePeriodViewModels timePeriodViewModelFactory,
            IGetReleasesFromTheDatabase releaseRepository)
        {
            _listOfCycleTimeItemsFactory = listOfCycleTimeItemsFactory;
            _ticketRepository = ticketRepository;
            _queryFactory = queryFactory;
            _timePeriodViewModelFactory = timePeriodViewModelFactory;
            _releaseRepository = releaseRepository;
        }

        public ActionResult Index(string timePeriod)
        {
            var cycleTimeQuery = _queryFactory.Build(timePeriod);
            var tickets = _ticketRepository.Get(cycleTimeQuery).ToArray();
            var cycleTimeTicketsList = _listOfCycleTimeItemsFactory.Build(tickets);
            var releaseRecords = _releaseRepository.GetAllReleases(cycleTimeQuery).OrderBy(r => r.StartedAt);
            var timePeriods = _timePeriodViewModelFactory.Build(cycleTimeQuery.Period);

            var releases = BuildReleasesViewModels(releaseRecords).ToArray();

            var ticketsCompletedCount = tickets.Count();
            var complexityPointsReleased = tickets.Sum(t => t.Size);
            var averageCycleTime = Math.Round(tickets.Any() ? tickets.Average(t => t.CycleTime.Days) : 0);

            return View(new ProductOwnerDashboardViewModel
                {
                    ReleaseCount = releases.Count(),
                    TicketsCompletedCount = ticketsCompletedCount,
                    ComplexityPointsReleased = complexityPointsReleased,
                    AverageCycleTime = averageCycleTime,
                    SelectedTimePeriodFriendlyName = "this week",
                    Tickets = cycleTimeTicketsList,
                    Releases = releases,
                    TimePeriods = timePeriods
                });
        }

        private static IEnumerable<ProductOwnerDashboardReleaseViewModel> BuildReleasesViewModels(IEnumerable<ReleaseRecord> releaseRecords)
        {
            return releaseRecords.Select(r => new ProductOwnerDashboardReleaseViewModel
                {
                    Id = r.Id,
                    Day = r.StartedAt.ToString("ddd"),
                    FormattedDate = r.StartedAt.ToString("dd MMM yyyy \"at\" HH:mm"),
                    ServiceNowId = r.ServiceNowId,
                    TicketCount = r.IncludedTickets.Count()
                });

        }
    }

    public class ProductOwnerDashboardViewModel
    {
        public int ReleaseCount { get; set; }

        public int TicketsCompletedCount { get; set; }

        public int ComplexityPointsReleased { get; set; }

        public double AverageCycleTime { get; set; }

        public string SelectedTimePeriodFriendlyName { get; set; }

        public IEnumerable<ProductOwnerDashboardReleaseViewModel> Releases { get; set; }

        public IEnumerable<ProductOwnerDashboardBlockagesViewModel> Blockages { get; set; }

        public IEnumerable<CycleTimeTicketItem> Tickets { get; set; }

        public CycleTimePeriodViewModel TimePeriods { get; set; }
    }

    public class ProductOwnerDashboardReleaseViewModel
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public string FormattedDate { get; set; }

        public int TicketCount { get; set; }

        public string ServiceNowId { get; set; }
    }

    public class ProductOwnerDashboardBlockagesViewModel
    {
         
    }
}

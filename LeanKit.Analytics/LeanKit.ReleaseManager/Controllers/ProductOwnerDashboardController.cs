using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using LeanKit.ReleaseManager.Models.TimePeriods;

namespace LeanKit.ReleaseManager.Controllers
{
    public class ProductOwnerDashboardController : Controller
    {
        private readonly IBuildListOfCycleTimeItems _listOfCycleTimeItemsFactory;
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly IMakeCycleTimeQueries _queryFactory;
        private readonly IMakeTimePeriodViewModels _timePeriodViewModelFactory;

        public ProductOwnerDashboardController(IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory, 
            IGetReleasedTicketsFromTheDatabase ticketRepository, 
            IMakeCycleTimeQueries queryFactory)
        {
            _listOfCycleTimeItemsFactory = listOfCycleTimeItemsFactory;
            _ticketRepository = ticketRepository;
            _queryFactory = queryFactory;
            _timePeriodViewModelFactory = new TimePeriodViewModelFactory();
        }

        public ActionResult Index(string timePeriod)
        {
            var cycleTimeQuery = _queryFactory.Build(timePeriod);
            var tickets = _ticketRepository.Get(cycleTimeQuery).ToArray();
            var cycleTimeTicketsList = _listOfCycleTimeItemsFactory.Build(tickets);

            var releases = new List<ProductOwnerDashboardReleaseViewModel>
                {
                    new ProductOwnerDashboardReleaseViewModel
                        {
                            Id = 1,
                            Day = "Tues",
                            FormattedDate = "11 Jun 2013 at 14:30",
                            TicketCount = 3,
                            ServiceNowId = "CHG0006063"
                        },
                    new ProductOwnerDashboardReleaseViewModel
                        {
                            Id = 2,
                            Day = "Weds",
                            FormattedDate = "12 Jun 2013 at 15:26",
                            TicketCount = 2,
                            ServiceNowId = "CHG0006093"
                        },
                    new ProductOwnerDashboardReleaseViewModel
                        {
                            Id = 3,
                            Day = "Thurs",
                            FormattedDate = "13 Jun 2013 at 16:07",
                            TicketCount = 1,
                            ServiceNowId = "CHG0006102"
                        },
                    new ProductOwnerDashboardReleaseViewModel
                        {
                            Id = 4,
                            Day = "Fri",
                            FormattedDate = "14 Jun 2013 at 15:25",
                            TicketCount = 3,
                            ServiceNowId = "CHG0006114"
                        },
                };

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
                    TimePeriods = _timePeriodViewModelFactory.Build(timePeriod)
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

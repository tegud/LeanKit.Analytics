﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.ReleaseManager.Models;
using LeanKit.ReleaseManager.Models.CycleTime;
using LeanKit.ReleaseManager.Models.Forecasting;
using LeanKit.Utilities.DateAndTime;
using Newtonsoft.Json;
using LeanKit.Utilities;
using Newtonsoft.Json.Converters;

namespace LeanKit.ReleaseManager.Controllers
{
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);
            response.Write(serializedObject);
        }
    }

    public class ProductOwnerDashboardController : Controller
    {
        private readonly IBuildListOfCycleTimeItems _listOfCycleTimeItemsFactory;
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly IMakeCycleTimeQueries _queryFactory;
        private readonly IMakeTimePeriodViewModels _timePeriodViewModelFactory;
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IGetBlockagesFromTheDatabase _blockageRepository;

        public ProductOwnerDashboardController(IBuildListOfCycleTimeItems listOfCycleTimeItemsFactory,
            IGetReleasedTicketsFromTheDatabase ticketRepository,
            IMakeCycleTimeQueries queryFactory,
            IMakeTimePeriodViewModels timePeriodViewModelFactory,
            IGetReleasesFromTheDatabase releaseRepository,
            IGetBlockagesFromTheDatabase blockageRepository)
        {
            _listOfCycleTimeItemsFactory = listOfCycleTimeItemsFactory;
            _ticketRepository = ticketRepository;
            _queryFactory = queryFactory;
            _timePeriodViewModelFactory = timePeriodViewModelFactory;
            _releaseRepository = releaseRepository;
            _blockageRepository = blockageRepository;
        }

        public JsonNetResult ReleaseInformation(DateTime start, DateTime end)
        {


            return new JsonNetResult { Data = null };
        }

        public ActionResult Index(string timePeriod)
        {
            var cycleTimeQuery = _queryFactory.Build(timePeriod);
            var tickets = _ticketRepository.Get(cycleTimeQuery).ToArray();
            var cycleTimeTicketsList = _listOfCycleTimeItemsFactory.Build(tickets);
            var releaseRecords = _releaseRepository.GetAllReleases(cycleTimeQuery).OrderBy(r => r.StartedAt);
            var timePeriods = _timePeriodViewModelFactory.Build(cycleTimeQuery.Period);
            var blockages = _blockageRepository.Get(cycleTimeQuery);

            var releases = BuildReleasesViewModels(releaseRecords).ToArray();

            var ticketsCompletedCount = tickets.Count();
            var complexityPointsReleased = tickets.Sum(t => t.Size);
            var averageCycleTime = Math.Round(tickets.Any() ? tickets.Average(t => t.CycleTime.Days) : 0);

            return View(new ProductOwnerDashboardViewModel
                {
                    ReleaseCount = releases.Count(),
                    RolledBackReleases = releases.Count(r => r.WasRolledBack),
                    TicketsCompletedCount = ticketsCompletedCount,
                    ComplexityPointsReleased = complexityPointsReleased,
                    AverageCycleTime = averageCycleTime,
                    SelectedTimePeriodFriendlyName = GetSelectedTimePeriodFriendlyName(cycleTimeQuery),
                    Tickets = cycleTimeTicketsList,
                    Releases = releases,
                    TimePeriods = timePeriods,
                    Blockages = BuildBlockageViewModels(blockages)
                });
        }

        public ActionResult Forecast(string timePeriod)
        {
            var cycleTimePeriods = GetForecastCycleTimePeriods(timePeriod);
            var forecast = new AverageForecast(_releaseRepository, _ticketRepository, 4).Forecast();

            return View(new ProductOwnerForecastViewModel
                {
                    TimePeriods = new CycleTimePeriodViewModel(cycleTimePeriods),
                    Forecast = forecast
                });
        }

        private static IEnumerable<CycleTimePeriod> GetForecastCycleTimePeriods(string timePeriod)
        {
            return new List<CycleTimePeriod>
                {
                    GetCycleTimePeriod("This Week", "ThisWeek", timePeriod == "ThisWeek" || string.IsNullOrWhiteSpace(timePeriod)), 
                    GetCycleTimePeriod("Next Week", "NextWeek", timePeriod == "NextWeek")
                };
        }

        private static CycleTimePeriod GetCycleTimePeriod(string label, string value, bool isSelected)
        {
            return new CycleTimePeriod { Label = label, Value = value, Selected = isSelected };
        }

        public ActionResult Graphs()
        {
            var weeks = new ListOfWeeks(DateTime.Now.Date.GetStartOfWeek().AddDays(-7 * 20), 20);

            var cycleTimeQuery = new CycleTimeQuery
            {
                Start = weeks.StartOfPeriod,
                End = weeks.EndOfPeriod
            };

            var tickets = _ticketRepository.Get(cycleTimeQuery).ToList();
            var allReleases = _releaseRepository.GetAllReleases(cycleTimeQuery).ToList();
            var totalTickets = 0;
            var totalComplexity = 0;
            var totalReleases = 0;

            var weekData = weeks.Select(w =>
                {
                    var ticketsInWeek = tickets.Where(t => t.Finished >= w.Start && t.Finished <= w.End).ToList();
                    var releasesInWeek = allReleases.Where(r => r.CompletedAt >= w.Start && r.CompletedAt <= w.End);
                    var complexity = ticketsInWeek.Sum(t => t.Size > 0 ? t.Size : 1);

                    return new
                        {
                            Start = w.Start,
                            End = w.End,
                            Tickets = totalTickets = totalTickets + ticketsInWeek.Count(),
                            Complexity = totalComplexity = totalComplexity + complexity,
                            Releases = totalReleases = totalReleases + releasesInWeek.Count()
                        };
                }).ToList();

            var graphData = JsonConvert.SerializeObject(new
            {
                Tickets = weekData.Select(t => new GraphEntry(t.Start, t.Tickets)),
                Complexity = weekData.Select(t => new GraphEntry(t.Start, t.Complexity)),
                Releases = weekData.Select(t => new GraphEntry(t.Start, t.Releases))
            }, new JavaScriptDateTimeConverter());

            return View("Graphs", new GraphViewModel
                {
                    GraphData = graphData
                });
        }

        private class GraphEntry
        {
            [JsonProperty(PropertyName = "time")]
            public object Date { get; private set; }

            [JsonProperty(PropertyName = "value")]
            public int Value { get; private set; }

            public GraphEntry(DateTime date, int value)
            {
                Value = value;
                Date = date;
            }
        }

        private static IEnumerable<ProductOwnerDashboardBlockagesViewModel> BuildBlockageViewModels(IEnumerable<TicketBlockage> blockages)
        {
            var groupedBlockages = blockages.GroupBy(b => b.Reason);

            return groupedBlockages.Select(b =>
                {
                    //var averageBlockageTime = b.Average(i => i.Duration.Hours);
                    var averageBlockageTime = 0;

                    return new ProductOwnerDashboardBlockagesViewModel
                        {
                            Title = b.Key,
                            AffectedTickets = b.Count(),
                            FormattedAverageBlockageTime = averageBlockageTime >= 1 ? "less than an hour" : averageBlockageTime.ToString("0")
                        };
                }).OrderByDescending(b => b.AffectedTickets);
        }

        private static string GetSelectedTimePeriodFriendlyName(CycleTimeQuery period)
        {
            if (period.Period == "this-week")
            {
                return "this week";
            }
            if (period.Period == "last-week")
            {
                return "last week";
            }

            return string.Format("for week starting {0}", period.Start.ToString("dd MMM"));
        }

        private static IEnumerable<ProductOwnerDashboardReleaseViewModel> BuildReleasesViewModels(IEnumerable<ReleaseRecord> releaseRecords)
        {
            return releaseRecords
                .OrderBy(r => r.PlannedDate)
                .ThenBy(r => r.StartedAt).Select(r => new ProductOwnerDashboardReleaseViewModel
                {
                    Id = r.Id,
                    Day = r.StartedAt > DateTime.MinValue ? r.StartedAt.ToString("ddd") : r.PlannedDate.ToString("ddd"),
                    FormattedDate = r.StartedAt > DateTime.MinValue ? r.StartedAt.ToString("dd MMM yyyy \"at\" HH:mm")
                     : r.PlannedDate.ToString("dd MMM yyyy \"at\" HH:mm"),
                    ServiceNowId = r.ServiceNowId,
                    TicketCount = r.IncludedTickets.Count(),
                    WasRolledBack = r.RollbackDate > DateTime.MinValue,
                    RollbackReason = r.RollbackReason
                });

        }
    }

    public class LineGraphWeek
    {
        public DateTime WeekStarts { get; set; }

        public int AverageCycleTime { get; set; }

        public int MaxCycleTime { get; set; }
    }

    public class CycleTimeGraphRow
    {
        public int Week { get; set; }

        public int TicketNumber { get; set; }

        public int CycleTime { get; set; }

        public string Label { get; set; }
    }

    public class GraphViewModel
    {
        public IHtmlString Data { get; set; }

        public IEnumerable<CycleTimeGraphRow> BoxChartItems { get; set; }

        public IEnumerable<LineGraphWeek> LineGraphItems { get; set; }

        public string GraphData { get; set; }
    }

    public class CycleTimeGraphWeek
    {
        public int WeekIndex { get; private set; }

        public IEnumerable<CycleTimeGraphWeekItem> Items { get; private set; }

        public string Label { get; set; }

        public CycleTimeGraphWeek(string label, int index, IEnumerable<Ticket> tickets)
        {
            Label = label;
            WeekIndex = index + 1;
            Items = tickets.Select((t, i) => new CycleTimeGraphWeekItem
                {
                    CycleTime = t.CycleTime.Days,
                    Index = i + 1
                });
        }
    }

    public class CycleTimeGraphWeekItem
    {
        public int Index { get; set; }

        public int CycleTime { get; set; }
    }

    public class ProductOwnerForecastViewModel
    {
        public CycleTimePeriodViewModel TimePeriods { get; set; }

        public PredictedThroughput Forecast { get; set; }
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

        public int RolledBackReleases { get; set; }
    }

    public class ProductOwnerDashboardReleaseViewModel
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public string FormattedDate { get; set; }

        public int TicketCount { get; set; }

        public string ServiceNowId { get; set; }

        public bool WasRolledBack { get; set; }

        public string RollbackReason { get; set; }
    }

    public class ProductOwnerDashboardBlockagesViewModel
    {
        public string Title { get; set; }

        public int AffectedTickets { get; set; }

        public string FormattedAverageBlockageTime { get; set; }
    }
}

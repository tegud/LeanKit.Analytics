using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.Forecasting
{
    public class AverageForecast : IPredictThroughput
    {
        private readonly IGetReleasesFromTheDatabase _releaseRepository;
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly int _weeksToForecast;

        public AverageForecast(IGetReleasesFromTheDatabase releaseRepository,
            IGetReleasedTicketsFromTheDatabase ticketRepository,
            int weeksToForecast)
        {
            _releaseRepository = releaseRepository;
            _ticketRepository = ticketRepository;
            _weeksToForecast = weeksToForecast;
        }

        public PredictedThroughput Forecast()
        {
            var weeks = new ListOfWeeks(DateTime.Now.Date.GetStartOfWeek().AddDays(-7*_weeksToForecast), _weeksToForecast);

            var cycleTimeQuery = new CycleTimeQuery
                {
                    Start = weeks.StartOfPeriod,
                    End = weeks.EndOfPeriod
                };

            var tickets = _ticketRepository.Get(cycleTimeQuery).ToList();
            var allReleases = _releaseRepository.GetAllReleases(cycleTimeQuery).ToList();

            var releasesByWeek = allReleases.GroupByWeek(weeks, (r, weekStart, weekEnd) => r.StartedAt >= weekStart && r.CompletedAt <= weekEnd).ToList();
            var ticketsByWeek = tickets.GroupByWeek(weeks, (r, weekStart, weekEnd) => r.Finished >= weekStart && r.Finished <= weekEnd).ToList();

            var forecastReleases = (int)Math.Round((double)releasesByWeek.Sum(r => r.Count()) / weeks.Count());

            var forecastTickets = (int)Math.Round((double)ticketsByWeek.Sum(w => w.Count()) / weeks.Count());

            var totalComplexityForPeriod = (double)ticketsByWeek.Sum(w => w.Where(t => t.Size > 0).Sum(t => t.Size));
            var forecastComplexity = (int)Math.Round(totalComplexityForPeriod / weeks.Count());

            return new PredictedThroughput(forecastReleases, forecastTickets, forecastComplexity);
        }
    }

    internal static class ForecastEnumberableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GroupByWeek<T>(this IEnumerable<T> collection, IEnumerable<Week> weeks,
                                                 Func<T, DateTime, DateTime, bool> grouping)
        {
            return weeks.Select(w => collection.Where(x => grouping(x, w.Start, w.End)));
        }
    }
}
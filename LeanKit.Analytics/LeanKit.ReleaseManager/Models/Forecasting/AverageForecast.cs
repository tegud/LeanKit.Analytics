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
        private readonly int _weeksToForecast;

        public AverageForecast(IGetReleasesFromTheDatabase releaseRepository, int weeksToForecast)
        {
            _releaseRepository = releaseRepository;
            _weeksToForecast = weeksToForecast;
        }

        public PredictedThroughput Forecast()
        {
            var startOfCurrentWeek = DateTime.Now.Date.GetStartOfWeek();

            var currentWeekStart = startOfCurrentWeek.AddDays(-7 * _weeksToForecast);
            var weeks = new List<Tuple<DateTime, DateTime>>();

            for (var x = 0; x < _weeksToForecast; x++)
            {
                weeks.Add(new Tuple<DateTime, DateTime>(currentWeekStart, currentWeekStart.AddDays(6)));

                currentWeekStart = currentWeekStart.AddDays(7);
            }

            var start = weeks.Min(d => d.Item1);
            var end = weeks.Max(d => d.Item2);

            var allReleases = _releaseRepository.GetAllReleases(new CycleTimeQuery
                {
                    Start = start,
                    End = end
                }).ToList();

            var releasesByWeek =
                weeks.Select(
                    w =>
                    allReleases.Where(r => r.StartedAt >= w.Item1 && r.CompletedAt <= w.Item2)
                               .Select(x => x)).ToList();

            var forecastReleases = (int)Math.Round((double)releasesByWeek.Sum(r => r.Count()) / releasesByWeek.Count());
            var forecastTickets = (int)Math.Round((double)releasesByWeek.Sum(r => r.Sum(x => x.IncludedTickets.Count())) / releasesByWeek.Count());
            var forecastComplexity = (int)Math.Round((double)releasesByWeek.Sum(r => r.Sum(x => x.IncludedTickets.Where(t => t.Size > 0).Sum(t => t.Size))) / releasesByWeek.Count());

            return new PredictedThroughput(forecastReleases, forecastTickets, forecastComplexity);
        }
    }
}
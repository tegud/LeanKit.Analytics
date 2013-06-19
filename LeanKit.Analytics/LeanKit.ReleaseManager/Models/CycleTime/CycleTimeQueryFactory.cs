using System;
using System.Collections.Generic;
using LeanKit.Data;
using LeanKit.ReleaseManager.ErrorHandling;
using LeanKit.ReleaseManager.Models.TimePeriods;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimeQueryFactory : IMakeCycleTimeQueries
    {
        private readonly IEnumerable<IMatchATimePeriod> _matchers;
        private readonly string _defaultValue;

        public CycleTimeQueryFactory(IConfigureTimePeriods configuration)
        {
            _matchers = configuration.Matchers;
            _defaultValue = configuration.DefaultValue;
        }

        public CycleTimeQuery Build(string timePeriod)
        {
            if (string.IsNullOrWhiteSpace(timePeriod))
            {
                timePeriod = _defaultValue;
            }

            var start = DateTime.MinValue;
            var end = DateTime.MinValue;
            var matchFound = false;

            foreach (var matcher in _matchers)
            {
                var match = matcher.GetTimeSpanIfMatch(timePeriod);

                if (match.IsMatch)
                {
                    start = match.Start;
                    end = match.End;
                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)
            {
                throw new UnknownTimePeriodException();
            }

            return new CycleTimeQuery
            {
                Start = start,
                End = end,
                Period = timePeriod
            };
        }
    }
}
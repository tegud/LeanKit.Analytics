using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LeanKit.Data;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimeQueryFactory : IMakeCycleTimeQueries
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        public CycleTimeQueryFactory(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public CycleTimeQuery Build(string timePeriod)
        {
            if (string.IsNullOrWhiteSpace(timePeriod))
            {
                timePeriod = "30";
            }

            var start = DateTime.MinValue;
            var end = DateTime.MinValue;

            var matchers = new IMatchATimePeriod[]
                {
                    new MatchWeekCommencingTimePeriod(_dateTimeWrapper),
                    new MatchDaysBeforeTimePeriod(_dateTimeWrapper),
                    new MatchKeywordTimePeriod(_dateTimeWrapper)
                };

            foreach (var matcher in matchers)
            {
                var match = matcher.GetTimeSpanIfMatch(timePeriod);

                if (match.IsMatch)
                {
                    start = match.Start;
                    end = match.End;
                    break;
                }
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
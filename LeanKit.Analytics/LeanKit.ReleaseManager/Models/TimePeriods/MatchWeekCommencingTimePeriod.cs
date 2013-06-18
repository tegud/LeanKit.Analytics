using System;
using System.Text.RegularExpressions;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class MatchWeekCommencingTimePeriod : IMatchATimePeriod
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        private readonly Regex _regex = new Regex("wc-(?<week>[0-9])+");

        public MatchWeekCommencingTimePeriod(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public TimePeriodMatch GetTimeSpanIfMatch(string timePeriod)
        {
            var match = _regex.Match(timePeriod);
            if (!match.Success)
            {
                return TimePeriodMatch.NotMatched;
            }

            var currentDate = _dateTimeWrapper.Now().Date;

            var dayOfWeekOffset = -(int)currentDate.DayOfWeek;
            var start = currentDate.AddDays(dayOfWeekOffset);

            var week = int.Parse(match.Groups["week"].Value);
            start = start.AddDays(-week * 7);

            var end = start.AddDays(6);

            return new TimePeriodMatch(start, end);
        }
    }

    public class MatchKeywordTimePeriod : IMatchATimePeriod
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        public MatchKeywordTimePeriod(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public TimePeriodMatch GetTimeSpanIfMatch(string timePeriod)
        {
            var match = new Regex("(this|last)-week").Match(timePeriod);
            if(!match.Success)
            {
                return TimePeriodMatch.NotMatched;
            }

            var currentDate = _dateTimeWrapper.Now().Date;
            var dayOfWeekOffset = -(int)currentDate.DayOfWeek;
            var start = currentDate.AddDays(dayOfWeekOffset);

            if (timePeriod == "last-week")
            {
                start = start.AddDays(-7);
            }

            var end = start.AddDays(6);

            return new TimePeriodMatch(start, end);
        }
    }

    public class MatchDaysBeforeTimePeriod : IMatchATimePeriod
    {
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        private readonly Regex _regex = new Regex("^[0-9]+$");

        public MatchDaysBeforeTimePeriod(IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }

        public TimePeriodMatch GetTimeSpanIfMatch(string timePeriod)
        {
            var match = _regex.Match(timePeriod);
            if (!match.Success)
            {
                return TimePeriodMatch.NotMatched;
            }

            var currentDate = _dateTimeWrapper.Now().Date;
            var start = _dateTimeWrapper.Now().Date.AddDays(-int.Parse(timePeriod));

            return new TimePeriodMatch(start, currentDate);
        }
    }
}
using System;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class TimePeriodMatch
    {
        public static TimePeriodMatch NotMatched = new TimePeriodMatch();

        public bool IsMatch { get; private set; }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public TimePeriodMatch(DateTime start, DateTime end)
        {
            IsMatch = true;
            Start = start;
            End = end;
        }

        private TimePeriodMatch()
        {
            IsMatch = false;
        }
    }
}
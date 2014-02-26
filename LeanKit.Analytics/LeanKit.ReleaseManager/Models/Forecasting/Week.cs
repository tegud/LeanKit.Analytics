using System;

namespace LeanKit.ReleaseManager.Models.Forecasting
{
    public class Week
    {
        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public Week(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
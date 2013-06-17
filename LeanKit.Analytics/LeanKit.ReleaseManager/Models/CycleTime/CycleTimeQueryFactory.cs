using System;
using LeanKit.Data;
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
                timePeriod = "0,30";
            }

            if (timePeriod == "all-time")
            {
                return new CycleTimeQuery
                    {
                        Period = timePeriod
                    };
            }

            var currentDate = _dateTimeWrapper.Now().Date;

            DateTime start;
            DateTime end;

            if (timePeriod.Contains("-week"))
            {
                var dayOfWeekOffset = -(int) currentDate.DayOfWeek;
                start = currentDate.AddDays(dayOfWeekOffset);

                if (timePeriod == "last-week")
                {
                    start = start.AddDays(-7);
                }

                end = start.AddDays(6);
            }
            else
            {
                var daysBefore = timePeriod.Split(',')[1];
                start = currentDate.AddDays(-int.Parse(daysBefore));
                end = currentDate;
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
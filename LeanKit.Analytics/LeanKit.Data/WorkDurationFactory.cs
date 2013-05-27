using System;
using System.Linq;

namespace LeanKit.Data
{
    public class WorkDayDefinition
    {
        public int Start { get; set; }

        public int End { get; set; }
    }

    public interface IWorkDurationFactory
    {
        WorkDuration Build(DateTime start, DateTime end);
    }

    public class WorkDurationFactory : IWorkDurationFactory
    {
        private readonly DateTime[] _holidays;
        private readonly WorkDayDefinition _workDayDefinition;

        public WorkDurationFactory(DateTime[] holidays, WorkDayDefinition workDayDefinition)
        {
            _holidays = holidays;
            _workDayDefinition = workDayDefinition;
        }

        public WorkDuration Build(DateTime start, DateTime end)
        {
            var days = CalculateWeekDays(start, end) - CalculateNumberOfHolidayDays(start, end);
            var hours = (days + 1) * 8;

            if(days == 0)
            {
                days = 1;
                hours = (int)(end - start).TotalHours;
            }
            else
            {
                hours -= (start.Hour - _workDayDefinition.Start) + (_workDayDefinition.End - end.Hour);
            }

            return new WorkDuration
                {
                    Days = days,
                    Hours = hours
                };
        }

        private static int CalculateWeekDays(DateTime start, DateTime end)
        {
            var days = (int) (end - start).TotalDays;
            var weeks = days / 7;
            var startDayOfWeek = (int) start.DayOfWeek;

            if (7 - (days % 7) <= startDayOfWeek)
            {
                days--;
            }
            if (7 - (days % 7) <= startDayOfWeek)
            {
                days--;
            }

            days -= weeks*2;
            return days;
        }

        private int CalculateNumberOfHolidayDays(DateTime start, DateTime end)
        {
            var coveredHolidays = _holidays.Count(holiday => holiday >= start && holiday <= end);
            return coveredHolidays;
        }
    }
}
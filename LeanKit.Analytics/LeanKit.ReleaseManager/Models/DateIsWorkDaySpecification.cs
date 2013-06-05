using System;

namespace LeanKit.ReleaseManager.Models
{
    public class DateIsWorkDaySpecification : IIdentifyWorkDays
    {
        public bool IsSatisfiedBy(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
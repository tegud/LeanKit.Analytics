using System;

namespace LeanKit.Utilities.DateAndTime
{
    public static class StartOfWeek
    {
        public static DateTime GetStartOfWeek(this DateTime date)
        {
            var dayOfWeekOffset = -(int) date.DayOfWeek;

            return date.AddDays(dayOfWeekOffset);
        }
    }
}
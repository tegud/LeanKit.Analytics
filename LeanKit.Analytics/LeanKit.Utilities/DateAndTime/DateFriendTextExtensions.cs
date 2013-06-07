using System;
using System.Collections.Generic;

namespace LeanKit.Utilities.DateAndTime
{
    public static class DateFriendTextExtensions
    {
        private static readonly Dictionary<double, string> FriendlyTexts = new Dictionary<double, string>
            {
                { -1, "Yesterday" },
                { 0, "Today" },
                { 1, "Tomorrow" }
            };

        public static string ToFriendlyText (this DateTime date, string dateFormat, string timeFormat)
        {
            if (date == DateTime.MinValue)
            {
                return string.Empty;
            }

            var todaysDate = DateTime.Now.Date;
            var daysDiff = (date.Date - todaysDate).TotalDays;

            string friendlyText;

            if (!FriendlyTexts.TryGetValue(daysDiff, out friendlyText))
            {
                friendlyText = date.ToString(dateFormat);
            }

            if (!string.IsNullOrWhiteSpace(timeFormat))
            {
                return friendlyText + date.ToString(timeFormat);
            }

            return friendlyText;
        }
    }
}
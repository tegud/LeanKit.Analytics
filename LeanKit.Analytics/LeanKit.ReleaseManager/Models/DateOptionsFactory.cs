using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.ReleaseManager.Controllers;
using LeanKit.Utilities.Tests.DateTimeExtensions;

namespace LeanKit.ReleaseManager.Models
{
    public class DateOptionsFactory : IMakeListsOfDateOptions
    {
        public IEnumerable<DateOption> BuildDateOptions(int numberOfDaysToDisplay)
        {
            var startDate = DateTime.Now.Date;
            var dates = new List<DateOption>(numberOfDaysToDisplay);
            var x = 0;

            while (dates.Count() < numberOfDaysToDisplay)
            {
                var currentDate = startDate.AddDays(x);

                x++;

                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                dates.Add(new DateOption
                    {
                        Date = currentDate,
                        FriendlyText = currentDate.ToFriendlyText("dddd", "")
                    });
            }

            return dates;
        }
    }
}
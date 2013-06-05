using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models
{
    public class DateOptionsFactory : IMakeListsOfDateOptions
    {
        private readonly IIdentifyWorkDays _dayIsWorkDaySpecification;

        public DateOptionsFactory(IIdentifyWorkDays dayIsWorkDaySpecification)
        {
            _dayIsWorkDaySpecification = dayIsWorkDaySpecification;
        }

        public IEnumerable<DateOption> BuildDateOptions(int numberOfDaysToDisplay)
        {
            var startDate = DateTime.Now.Date;
            var dates = new List<DateOption>(numberOfDaysToDisplay);
            var x = 0;

            while (dates.Count() < numberOfDaysToDisplay)
            {
                var currentDate = startDate.AddDays(x);

                x++;

                if (!_dayIsWorkDaySpecification.IsSatisfiedBy(currentDate))
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
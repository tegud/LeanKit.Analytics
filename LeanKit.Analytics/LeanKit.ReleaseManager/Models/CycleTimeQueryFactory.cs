using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models
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
            if (string.IsNullOrWhiteSpace(timePeriod) || timePeriod == "all-time")
            {
                return CycleTimeQuery.Empty;
            }

            var currentDate = _dateTimeWrapper.Now().Date;
            var dayOfWeekOffset = -(int) currentDate.DayOfWeek;

            var start = currentDate.AddDays(dayOfWeekOffset);
            return new CycleTimeQuery
                {
                    Start = start,
                    End = start.AddDays(6)
                };
        }
    }
}
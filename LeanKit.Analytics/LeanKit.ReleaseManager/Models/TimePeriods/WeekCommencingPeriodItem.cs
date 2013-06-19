using System;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class WeekCommencingPeriodItem : IDefineATimePeriodItem
    {
        private readonly int _weeksBefore;

        public WeekCommencingPeriodItem(int weeksBefore)
        {
            _weeksBefore = weeksBefore;
        }

        public string GetLabel()
        {
            var currentDate = DateTime.Now.Date;
            var dayOfWeekOffset = -(int)currentDate.DayOfWeek;
            var start = currentDate.AddDays(dayOfWeekOffset).AddDays(_weeksBefore * 7);

            return start.ToString("\"W/C\" dd MMM");
        }

        public string GetValue()
        {
            return string.Format("wc-{0}", -_weeksBefore);
        }
    }
}
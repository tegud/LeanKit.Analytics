using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class TimePeriodViewModelFactory : IMakeTimePeriodViewModels
    {
        private readonly IEnumerable<IDefineATimePeriodItem> _periods = new IDefineATimePeriodItem[]
            {
                new StaticPeriodItem("This Week", "this-week"),
                new StaticPeriodItem("Last Week", "last-week"),
                new WeekCommencingPeriodItem(-14), 
                new WeekCommencingPeriodItem(-21), 
                new WeekCommencingPeriodItem(-28), 
                new WeekCommencingPeriodItem(-35), 
                new StaticPeriodItem("All Time", "all-time")
            };


        public CycleTimePeriodViewModel Build(string selectedPeriod)
        {
            var periods = _periods.Select(p =>
                {
                    var value = p.GetValue();
                    return new CycleTimePeriod
                        {
                            Label = p.GetLabel(),
                            Value = value,
                            Selected = value == selectedPeriod
                        };
                });

            return new CycleTimePeriodViewModel(periods);
        }

    }

    public class StaticPeriodItem : IDefineATimePeriodItem
    {
        private readonly string _label;
        private readonly string _value;

        public StaticPeriodItem(string label, string value)
        {
            _label = label;
            _value = value;
        }

        public string GetLabel()
        {
            return _label;
        }

        public string GetValue()
        {
            return _value;
        }
    }

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
            var start = currentDate.AddDays(dayOfWeekOffset).AddDays(-_weeksBefore);

            return start.ToString("\"W/C\" dd MMM");
        }

        public string GetValue()
        {
            return string.Format("{0},7", -_weeksBefore);
        }
    }

    public class LastXDaysPeriodItem : IDefineATimePeriodItem
    {
        private readonly int _daysBefore;

        public LastXDaysPeriodItem(int daysBefore)
        {
            _daysBefore = daysBefore;
        }

        public string GetLabel()
        {
            return string.Format("Last {0} Days", _daysBefore);
        }

        public string GetValue()
        {
            return string.Format("0,{0}", _daysBefore);
        }
    }

    public interface IDefineATimePeriodItem
    {
        string GetLabel();

        string GetValue();
    }
}
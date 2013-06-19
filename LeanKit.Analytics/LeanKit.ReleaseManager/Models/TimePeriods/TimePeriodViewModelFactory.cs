using System.Collections.Generic;
using System.Linq;
using LeanKit.ReleaseManager.Models.CycleTime;

namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class TimePeriodViewModelFactory : IMakeTimePeriodViewModels
    {
        private readonly IEnumerable<IDefineATimePeriodItem> _periods = new IDefineATimePeriodItem[]
            {
                new StaticPeriodItem("This Week", "this-week"),
                new StaticPeriodItem("Last Week", "last-week"),
                new WeekCommencingPeriodItem(-2), 
                new WeekCommencingPeriodItem(-3), 
                new WeekCommencingPeriodItem(-4), 
                new WeekCommencingPeriodItem(-5), 
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
}
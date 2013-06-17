using System.Collections.Generic;
using System.Linq;
using LeanKit.ReleaseManager.Models.TimePeriods;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public class CycleTimePeriodViewModelFactory : IMakeTimePeriodViewModels
    {
        private readonly IEnumerable<IDefineATimePeriodItem> _periods = new IDefineATimePeriodItem[]
            {
                new StaticPeriodItem("This Week", "this-week"),
                new StaticPeriodItem("Last Week", "last-week"),
                new LastXDaysPeriodItem(30), 
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
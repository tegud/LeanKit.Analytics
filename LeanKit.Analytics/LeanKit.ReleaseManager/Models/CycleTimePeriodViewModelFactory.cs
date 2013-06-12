using System.Collections.Generic;
using System.Linq;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimePeriodViewModelFactory : IMakeTimePeriodViewModels
    {
        private const string DAYS_FORMAT_STRING = "Last {0} Days";

        private readonly IEnumerable<string> _periodOrder = new[] { "this-week", "last-week", "30", "all-time" };

        private readonly Dictionary<string, string> _periods = new Dictionary<string, string>
            {
                {"last-week", "Last Week"},
                {"this-week", "This Week"},
                {"all-time", "All Time"}
            };

        public CycleTimePeriodViewModel Build()
        {
            var periods = _periodOrder.Select(p => new CycleTimePeriod
                {
                    Label = GetLabel(p),
                    Value = p
                });

            return new CycleTimePeriodViewModel(periods);
        }

        private string GetLabel(string p)
        {
            return _periods.ContainsKey(p) ? _periods[p] : string.Format(DAYS_FORMAT_STRING, p);
        }
    }
}
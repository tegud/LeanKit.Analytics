using System.Collections;
using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimePeriodViewModel : IEnumerable<CycleTimePeriod>
    {
        private readonly IEnumerable<CycleTimePeriod> _timePeriods;

        public CycleTimePeriodViewModel(IEnumerable<CycleTimePeriod> timePeriods)
        {
            _timePeriods = timePeriods;
        }

        public IEnumerator<CycleTimePeriod> GetEnumerator()
        {
            return _timePeriods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
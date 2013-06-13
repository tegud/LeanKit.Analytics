using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.Graphs
{
    public class ActivityBreakdown
    {
        public IEnumerable<ActivityBreakdownItem> Activities { get; private set; }

        public ActivityBreakdown(IEnumerable<ActivityBreakdownItem> activities)
        {
            Activities = activities;
        }
    }
}
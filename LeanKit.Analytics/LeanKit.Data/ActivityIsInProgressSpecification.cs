using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data
{
    public class ActivityIsInProgressSpecification : IActivitySpecification
    {
        private readonly IEnumerable<string> _inProgressActivities = new List<string>
            {
                "DEV WIP", "DEV DONE", "READY FOR TEST", "TEST WIP", "READY FOR RELEASE", "LIVE"
            };

        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return _inProgressActivities.Contains(activity.Title.ToUpper());
        }
    }
}
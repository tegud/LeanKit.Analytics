using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data.Activities
{
    public class ActivityIsInProgressSpecification : IActivitySpecification
    {
        private readonly IEnumerable<string> _inProgressActivities = new List<string>
            {
                "DEV WIP", "DEV DONE", "READY TO TEST", "TEST WIP", "READY TO RELEASE"
            };

        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return _inProgressActivities.Contains(activity.Title.ToUpper());
        }
    }
}
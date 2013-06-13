using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data
{
    public class TicketStartDateFactory : ICalculateTicketMilestone
    {
        private readonly IActivitySpecification _activityIsInProgressSpecification;

        public TicketStartDateFactory(IActivitySpecification activityIsInProgressSpecification)
        {
            _activityIsInProgressSpecification = activityIsInProgressSpecification;
        }

        public DateTime CalculateMilestone(IEnumerable<TicketActivity> ticketActivities)
        {
            var startedActivities = ticketActivities.Where(a => a.Started > DateTime.MinValue).OrderBy(a => a.Started);

            var firstInProgressActivity = startedActivities.FirstOrDefault(_activityIsInProgressSpecification.IsSatisfiedBy);
            var started = firstInProgressActivity == null ? DateTime.MinValue : firstInProgressActivity.Started;
            return started;
        }
    }
}
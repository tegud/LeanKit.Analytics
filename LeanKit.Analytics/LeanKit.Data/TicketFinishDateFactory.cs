using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data
{
    public class TicketFinishDateFactory : ICalculateTicketMilestone
    {
        private readonly IActivitySpecification _activityIsLiveSpecification;

        public TicketFinishDateFactory(IActivitySpecification activityIsLiveSpecification)
        {
            _activityIsLiveSpecification = activityIsLiveSpecification;
        }

        public DateTime CalculateMilestone(IEnumerable<TicketActivity> ticketActivities)
        {
            var liveActivity = ticketActivities.FirstOrDefault(_activityIsLiveSpecification.IsSatisfiedBy);
            var finished = liveActivity != null ? liveActivity.Started : DateTime.MinValue;
            return finished;
        }
    }
}
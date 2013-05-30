using System;
using System.Linq;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class TicketFactory : ITicketFactory
    {
        private readonly TicketActivitiesFactory _ticketActivitiesFactory;
        private readonly IWorkDurationFactory _workDurationFactory;
        private readonly IActivitySpecification _activityIsInProgressSpecification;

        public TicketFactory(TicketActivitiesFactory ticketActivitiesFactory, 
                             IWorkDurationFactory workDurationFactory, 
                             IActivitySpecification activityIsInProgressSpecification)
        {
            _ticketActivitiesFactory = ticketActivitiesFactory;
            _workDurationFactory = workDurationFactory;
            _activityIsInProgressSpecification = activityIsInProgressSpecification;
        }

        public Ticket Build(LeankitBoardCard card)
        {
            var ticketActivities = _ticketActivitiesFactory.Build(card).ToArray();

            var firstInProgressActivity = ticketActivities.FirstOrDefault(_activityIsInProgressSpecification.IsSatisfiedBy);

            var started = firstInProgressActivity == null ? DateTime.MinValue : firstInProgressActivity.Started;

            var liveActivity = ticketActivities.FirstOrDefault(a => a.Title.ToUpper() == "LIVE");
            var finished = liveActivity != null ? liveActivity.Started : DateTime.MinValue;

            var duration = _workDurationFactory.Build(started, liveActivity != null ? finished : DateTime.Now);

            return new Ticket
                {
                    Id = card.Id,
                    Title = card.Title,
                    Started = started,
                    Finished = finished,
                    Activities = ticketActivities,
                    CycleTime = duration
                };
        }
    }
}
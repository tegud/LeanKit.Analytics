using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ITicketActivitiesFactory _ticketActivitiesFactory;
        private readonly ICalculateWorkDuration _ticketCycleTimeDurationFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly IActivitySpecification _activityIsLiveSpecification;

        public TicketFactory(ITicketActivitiesFactory ticketActivitiesFactory,
                             ICalculateWorkDuration ticketCycleTimeDurationFactory, 
                             ICalculateTicketMilestone ticketStartDateFactory, 
                             IActivitySpecification activityIsLiveSpecification)
        {
            _ticketActivitiesFactory = ticketActivitiesFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
            _ticketStartDateFactory = ticketStartDateFactory;
            _activityIsLiveSpecification = activityIsLiveSpecification;
        }

        public Ticket Build(LeankitBoardCard card)
        {
            var ticketActivities = _ticketActivitiesFactory.Build(card).ToArray();

            var started = _ticketStartDateFactory.CalculateStart(ticketActivities);
            var finished = CalculateFinish(ticketActivities, _activityIsLiveSpecification);
            var duration = _ticketCycleTimeDurationFactory.CalculateDuration(started, finished);

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

        private static DateTime CalculateFinish(IEnumerable<TicketActivity> ticketActivities, IActivitySpecification activityIsLiveSpecification)
        {
            var liveActivity = ticketActivities.FirstOrDefault(activityIsLiveSpecification.IsSatisfiedBy);
            var finished = liveActivity != null ? liveActivity.Started : DateTime.MinValue;
            return finished;
        }
    }

    public class ActivityIsLiveSpecification : IActivitySpecification
    {
        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return activity.Title.ToUpper() == "LIVE";
        }
    }

    public class TicketStartDateFactory : ICalculateTicketMilestone
    {
        private readonly IActivitySpecification _activityIsInProgressSpecification;

        public TicketStartDateFactory(IActivitySpecification activityIsInProgressSpecification)
        {
            _activityIsInProgressSpecification = activityIsInProgressSpecification;
        }

        public DateTime CalculateStart(IEnumerable<TicketActivity> ticketActivities)
        {
            var firstInProgressActivity = ticketActivities.FirstOrDefault(_activityIsInProgressSpecification.IsSatisfiedBy);
            var started = firstInProgressActivity == null ? DateTime.MinValue : firstInProgressActivity.Started;
            return started;
        }
    }

    public interface ICalculateTicketMilestone
    {
        DateTime CalculateStart(IEnumerable<TicketActivity> ticketActivities);
    }
}
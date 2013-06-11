using System.Linq;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.SQL
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ICalculateWorkDuration _ticketCycleTimeDurationFactory;
        private readonly IFindTheCurrentActivity _ticketCurrentActivityFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly ICalculateTicketMilestone _ticketFinishDateFactory;
        private readonly ICreateTicketActivities _ticketActivityFactory;

        public TicketFactory(ICalculateTicketMilestone ticketStartDateFactory, 
            ICalculateTicketMilestone ticketFinishDateFactory, 
            ICreateTicketActivities ticketActivityFactory, 
            ICalculateWorkDuration ticketCycleTimeDurationFactory, 
            IFindTheCurrentActivity ticketCurrentActivityFactory)
        {
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketFinishDateFactory = ticketFinishDateFactory;
            _ticketActivityFactory = ticketActivityFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
            _ticketCurrentActivityFactory = ticketCurrentActivityFactory;
        }

        public Ticket Build(TicketRecord ticket)
        {
            var activities = ticket.Activities.SelectWithNext((current, next) => _ticketActivityFactory.Build(current, next)).ToArray();

            var started = _ticketStartDateFactory.CalculateMilestone(activities);
            var finished = _ticketFinishDateFactory.CalculateMilestone(activities);
            var duration = _ticketCycleTimeDurationFactory.CalculateDuration(started, finished);
            var currentActivity = _ticketCurrentActivityFactory.Build(activities);

            return new Ticket
                {
                    Id = ticket.Id,
                    ExternalId = ticket.ExternalId,
                    Title = ticket.Title,
                    Started = started,
                    Finished = finished,
                    CycleTime = duration,
                    Size = ticket.Size,
                    Activities = activities,
                    CurrentActivity = currentActivity
                };
        }
    }
}
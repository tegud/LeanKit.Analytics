using System.Linq;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.SQL
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ICalculateWorkDuration _workDurationFactory;
        private readonly ICalculateWorkDuration _ticketCycleTimeDurationFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly ICalculateTicketMilestone _ticketFinishDateFactory;
        private readonly ICreateTicketActivities _ticketActivityFactory;

        public TicketFactory(ICalculateWorkDuration workDurationFactory, 
            ICalculateTicketMilestone ticketStartDateFactory, 
            ICalculateTicketMilestone ticketFinishDateFactory, 
            ICreateTicketActivities ticketActivityFactory, 
            ICalculateWorkDuration ticketCycleTimeDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketFinishDateFactory = ticketFinishDateFactory;
            _ticketActivityFactory = ticketActivityFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
        }

        public ICalculateWorkDuration WorkDurationFactory
        {
            get { return _workDurationFactory; }
        }

        public Ticket Build(TicketRecord ticket)
        {
            var activities = ticket.Activities.SelectWithNext((current, next) => _ticketActivityFactory.Build(current, next)).ToArray();

            var started = _ticketStartDateFactory.CalculateMilestone(activities);
            var finished = _ticketFinishDateFactory.CalculateMilestone(activities);
            var duration = _ticketCycleTimeDurationFactory.CalculateDuration(started, finished);

            return new Ticket
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Started = started,
                    Finished = finished,
                    CycleTime = duration,
                    Activities = activities
                };
        }
    }
}
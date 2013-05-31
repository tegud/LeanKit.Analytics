using System.Linq;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ITicketActivitiesFactory _ticketActivitiesFactory;
        private readonly ICalculateWorkDuration _ticketCycleTimeDurationFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly ICalculateTicketMilestone _ticketFinishDateFactory;

        public TicketFactory(ITicketActivitiesFactory ticketActivitiesFactory,
                             ICalculateWorkDuration ticketCycleTimeDurationFactory,
                             ICalculateTicketMilestone ticketStartDateFactory,
                             ICalculateTicketMilestone ticketFinishDateFactory)
        {
            _ticketActivitiesFactory = ticketActivitiesFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketFinishDateFactory = ticketFinishDateFactory;
        }

        public Ticket Build(LeankitBoardCard card)
        {
            var ticketActivities = _ticketActivitiesFactory.Build(card).ToArray();

            var started = _ticketStartDateFactory.CalculateMilestone(ticketActivities);
            var finished = _ticketFinishDateFactory.CalculateMilestone(ticketActivities);
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
    }
}
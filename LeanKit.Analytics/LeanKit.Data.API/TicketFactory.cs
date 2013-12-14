using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IMakeTicketBlockages _ticketBlockagesFactory;
        private readonly IApiCaller _apiCaller;

        public TicketFactory(ITicketActivitiesFactory ticketActivitiesFactory, ICalculateWorkDuration ticketCycleTimeDurationFactory, ICalculateTicketMilestone ticketStartDateFactory, ICalculateTicketMilestone ticketFinishDateFactory, IMakeTicketBlockages ticketBlockagesFactory, IApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
            _ticketActivitiesFactory = ticketActivitiesFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketFinishDateFactory = ticketFinishDateFactory;
            _ticketBlockagesFactory = ticketBlockagesFactory;
        }

        public Ticket Build(LeankitBoardCard card)
        {
            var cardHistory = _apiCaller.GetCardHistory(card.Id).ToArray();

            var ticketActivities = _ticketActivitiesFactory.Build(cardHistory).ToArray();
            var ticketBlockages = _ticketBlockagesFactory.Build(cardHistory).ToArray();

            var started = _ticketStartDateFactory.CalculateMilestone(ticketActivities);
            var finished = _ticketFinishDateFactory.CalculateMilestone(ticketActivities);
            var duration = _ticketCycleTimeDurationFactory.CalculateDuration(started, finished);

            var allUsersForTicket = ticketActivities.Where(a => a.AssignedUser != TicketAssignedUser.UnAssigned).Select(a => a.AssignedUser);

            var ticketAssignedUsers = card.AssignedUsers.Select(user => allUsersForTicket.FirstOrDefault(u => u.Id == user.AssignedUserId));

            var tags = card.Tags.Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries);

            var projectTags = tags.Where(t => t.StartsWith("a", true, CultureInfo.InvariantCulture));

            return new Ticket
                {
                    Id = card.Id,
                    Title = card.Title,
                    ExternalId = card.ExternalCardID,
                    Started = started,
                    Finished = finished,
                    Activities = ticketActivities,
                    CycleTime = duration,
                    Size = card.Size,
                    AssignedUsers = ticketAssignedUsers,
                    Blockages = ticketBlockages,
                    Projects = projectTags.Select(t => new TicketProject() { ID = t })
                };
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.API
{
    public class TicketActivitiesFactory : ITicketActivitiesFactory
    {
        private readonly IApiCaller _apiCaller;
        private readonly ITicketActivityFactory _ticketActivityFactory;
        private readonly ReleventHistoryTypeSpecification _releventHistoryTypeSpecification;

        public TicketActivitiesFactory(IApiCaller apiCaller, ITicketActivityFactory ticketActivityFactory)
        {
            _apiCaller = apiCaller;
            _ticketActivityFactory = ticketActivityFactory;
            _releventHistoryTypeSpecification = new ReleventHistoryTypeSpecification();
        }

        public IEnumerable<TicketActivity> Build(LeankitBoardCard card)
        {
            var cardHistory = _apiCaller.GetCardHistory(card.Id).ToArray();

            var cardMoveEvents = cardHistory.Where(_releventHistoryTypeSpecification.IsSpecified);

            var ticketActivities = cardMoveEvents.SelectWithNext(_ticketActivityFactory.Build);

            return ticketActivities;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.API
{
    public class TicketActivitiesFactory : ITicketActivitiesFactory
    {
        private readonly ICreateTicketActivities _ticketActivityFactory;
        private readonly HistoryIsReleventToActivitiesSpecification _historyIsReleventToActivitiesSpecification;

        public TicketActivitiesFactory(ICreateTicketActivities ticketActivityFactory, HistoryIsReleventToActivitiesSpecification historyIsReleventToActivitiesSpecification)
        {
            _ticketActivityFactory = ticketActivityFactory;
            _historyIsReleventToActivitiesSpecification = historyIsReleventToActivitiesSpecification;
        }

        public IEnumerable<TicketActivity> Build(IEnumerable<LeanKitCardHistory> cardHistory)
        {
            var cardMoveEvents = cardHistory.Where(_historyIsReleventToActivitiesSpecification.IsSpecified);

            var ticketActivities = cardMoveEvents.SelectWithPreviousAndNext(_ticketActivityFactory.Build);

            return ticketActivities;
        }
    }
}
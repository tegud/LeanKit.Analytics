using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Data.Activities;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data
{
    public class AllTicketsForBoardFactory
    {
        private readonly IApiCaller _apiCaller;
        private readonly IActivitySpecification _activityIsInProgressSpecification;
        private readonly ITicketActivityFactory _ticketActivityFactory;

        public AllTicketsForBoardFactory(IApiCaller apiCaller, 
            IActivitySpecification activityIsInProgressSpecification,
            ITicketActivityFactory ticketActivityFactory)
        {
            _activityIsInProgressSpecification = activityIsInProgressSpecification;
            _apiCaller = apiCaller;
            _ticketActivityFactory = ticketActivityFactory;
        }

        public AllTicketsForBoard Build()
        {
            var board = _apiCaller.GetBoard();

            var allTicketsFromBoard = board.Lanes.SelectMany(c => c.Cards).ToList();

            var allArchiveCards = GetArchiveCards();

            allTicketsFromBoard.AddRange(allArchiveCards);

            return new AllTicketsForBoard
                {
                    Tickets = allTicketsFromBoard.Select(BuildTicket)
                };
        }

        private IEnumerable<LeankitBoardCard> GetArchiveCards()
        {
            var archive = _apiCaller.GetBoardArchive();
            var allArchiveLanes = archive.FlattenHierarchy(y => y.ChildLanes);

            return allArchiveLanes.SelectMany(z => z.Lane.Cards ?? new List<LeankitBoardCard>().ToArray());
        }

        private Ticket BuildTicket(LeankitBoardCard card)
        {
            var ticketActivities = BuildTicketActivities(card).ToArray();

            var firstInProgressActivity = ticketActivities.FirstOrDefault(activity => _activityIsInProgressSpecification.IsSatisfiedBy(activity));

            var started = firstInProgressActivity == null ? DateTime.MinValue : firstInProgressActivity.Started;

            return new Ticket
                {
                    Id = card.Id,
                    Title = card.Title,
                    Started = started,
                    Activities = ticketActivities
                };
        }

        private IEnumerable<TicketActivity> BuildTicketActivities(LeankitBoardCard c)
        {
            var cardHistory = _apiCaller.GetCardHistory(c.Id).ToArray();

            var cardMoveEvents = cardHistory.Where(HistoryTypeIsReleventToCardActivity);

            return cardMoveEvents.SelectWithNext(_ticketActivityFactory.Build);
        }

        private static bool HistoryTypeIsReleventToCardActivity(LeanKitCardHistory historyItem)
        {
            return historyItem.Type == "CardCreationEventDTO" || historyItem.Type == "CardMoveEventDTO";
        }
    }
}
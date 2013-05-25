using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;

namespace LeanKit.Data
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> FlattenHierarchy<T>(this T node, Func<T, IEnumerable<T>> getChildEnumerator)
        {
            yield return node;
            if (getChildEnumerator(node) != null)
            {
                foreach (var child in getChildEnumerator(node))
                {
                    foreach (var childOrDescendant in child.FlattenHierarchy(getChildEnumerator))
                    {
                        yield return childOrDescendant;
                    }
                }
            }
        }
    }

    public class AllTicketsForBoardFactory
    {
        private readonly IApiCaller _apiCaller;
        private readonly ActivityIsInProgressSpecification _activityIsInProgressSpecification;

        public AllTicketsForBoardFactory(IApiCaller apiCaller, ActivityIsInProgressSpecification activityIsInProgressSpecification)
        {
            _activityIsInProgressSpecification = activityIsInProgressSpecification;
            _apiCaller = apiCaller;
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

        private static DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", string.Empty));
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
            var count = cardHistory.Count();
            var cardMoveEvents = cardHistory.Where(h => h.Type == "CardCreationEventDTO" || h.Type == "CardMoveEventDTO");

            var ticketActivities = cardMoveEvents.Select((h, i) =>
                {
                    var isCurrent = count - 1 == i;
                    var finished = DateTime.MinValue;

                    if (!isCurrent)
                    {
                        finished = ParseLeanKitHistoryDateTime(cardHistory.ElementAt(i + 1).DateTime);
                    }

                    return new TicketActivity
                        {
                            Title = h.ToLaneTitle,
                            Started = ParseLeanKitHistoryDateTime(h.DateTime),
                            Finished = finished
                        };
                });

            return ticketActivities;
        }
    }

    public class ActivityIsInProgressSpecification : IActivitySpecification
    {
        private readonly IEnumerable<string> _inProgressActivities = new List<string>
            {
                "DEV WIP", "DEV DONE", "READY TO TEST", "TEST WIP", "READY TO RELEASE"
            };

        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return _inProgressActivities.Contains(activity.Title.ToUpper());
        }
    }

    public interface IActivitySpecification
    {
        bool IsSatisfiedBy(TicketActivity activity);
    }
}
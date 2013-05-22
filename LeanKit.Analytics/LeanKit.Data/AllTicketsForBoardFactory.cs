using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;

namespace LeanKit.Data
{
    public class AllTicketsForBoardFactory
    {
        private readonly IApiCaller _apiCaller;

        public AllTicketsForBoardFactory(IApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        private DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", string.Empty));
        }

        public AllTicketsForBoard Build()
        {
            var board = _apiCaller.GetBoard();
            var allTicketsFromBoard = board.Lanes.SelectMany(c => c.Cards);

            return new AllTicketsForBoard
                {
                    Tickets = allTicketsFromBoard.Select(c =>
                        {
                            var cardHistory = _apiCaller.GetCardHistory(c.Id);
                            var cardMoveEvents = cardHistory.Where(h => h.Type == "CardCreationEventDTO" || h.Type == "CardMoveEventDTO");

                            var ticketActivities = cardMoveEvents.Select((h, i) =>
                                {
                                    var isCurrent = cardHistory.Count() - 1 == i;
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

                            var firstInProgressActivity = ticketActivities.FirstOrDefault(a =>
                                {
                                    var upperTitle = a.Title.ToUpper();

                                    return upperTitle == "DEV WIP"
                                           || upperTitle == "DEV DONE"
                                           || upperTitle == "READY TO TEST"
                                           || upperTitle == "TEST WIP"
                                           || upperTitle == "READY TO RELEASE";
                                });

                            var started = firstInProgressActivity == null ? DateTime.MinValue : firstInProgressActivity.Started;

                            return new Ticket
                                {
                                    Id = c.Id,
                                    Title = c.Title,
                                    Started = started,
                                    Activities = ticketActivities
                                };
                        })
                };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.APIClient.API;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.API
{
    public class TicketBlockagesFactory : IMakeTicketBlockages
    {
        public IEnumerable<TicketBlockage> Build(IEnumerable<LeanKitCardHistory> cardHistory)
        {
            var releventHistoryItems = cardHistory.Where(history => history.Type == "CardBlockedEventDTO");

            if (!releventHistoryItems.Any())
            {
                return new List<TicketBlockage>(0);
            }

            var blockages = releventHistoryItems.SelectWithPreviousAndNext((current, previous, next) => new 
                {
                    Started = ParseLeanKitHistoryDateTime(current.DateTime), 
                    Finished = next == null ? DateTime.MinValue : ParseLeanKitHistoryDateTime(next.DateTime), 
                    Reason = current.Comment,
                    IsBlockStart = current.IsBlocked
                });

            return blockages.Where(b => b.IsBlockStart).Select(b => new TicketBlockage
                {
                    Started = b.Started,
                    Finished = b.Finished,
                    Reason = b.Reason
                });
        }

        private static DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", String.Empty));
        }
    }
}
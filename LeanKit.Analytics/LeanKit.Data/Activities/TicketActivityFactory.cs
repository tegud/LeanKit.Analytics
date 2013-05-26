using System;
using LeanKit.APIClient.API;

namespace LeanKit.Data.Activities
{
    public class TicketActivityFactory : ITicketActivityFactory
    {
        public TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory nextItem)
        {
            var finished = DateTime.MinValue;
            if (nextItem != null)
            {
                finished = ParseLeanKitHistoryDateTime(nextItem.DateTime);
            }

            return new TicketActivity
                {
                    Title = historyItem.ToLaneTitle,
                    Started = ParseLeanKitHistoryDateTime(historyItem.DateTime),
                    Finished = finished
                };
        }

        private static DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", String.Empty));
        }
    }
}
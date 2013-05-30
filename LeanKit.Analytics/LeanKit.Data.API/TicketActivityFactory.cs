using System;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class TicketActivityFactory : ITicketActivityFactory
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketActivityFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory nextItem)
        {
            var started = ParseLeanKitHistoryDateTime(historyItem.DateTime);
            var finished = DateTime.MinValue;

            if (nextItem != null)
            {
                finished = ParseLeanKitHistoryDateTime(nextItem.DateTime);
            }

            return new TicketActivity
                {
                    Title = (historyItem.IsBlocked ? "Blocked: " : "") + historyItem.ToLaneTitle,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished == DateTime.MinValue ? DateTime.Now : finished)
                };
        }

        private static DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", String.Empty));
        }
    }
}
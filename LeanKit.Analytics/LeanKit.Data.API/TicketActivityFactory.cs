using System;
using System.Net.Mail;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class TicketActivityFactory : ICreateTicketActivities
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketActivityFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory nextItem)
        {
            throw new NotImplementedException();
        }

        public TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory previousHistoryItem, LeanKitCardHistory nextItem)
        {
            var started = ParseLeanKitHistoryDateTime(historyItem.DateTime);
            var finished = DateTime.MinValue;

            if (nextItem != null)
            {
                finished = ParseLeanKitHistoryDateTime(nextItem.DateTime);
            }

            var ticketActivityAssignedUser = AssignedUser(historyItem);

            if (ticketActivityAssignedUser == TicketActivityAssignedUser.UnAssigned 
                && previousHistoryItem != null 
                && !historyItem.IsUnassigning)
            {
                ticketActivityAssignedUser = AssignedUser(previousHistoryItem);
            }

            return new TicketActivity
                {
                    Title = (historyItem.IsBlocked ? "Blocked: " : "") + historyItem.ToLaneTitle,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished == DateTime.MinValue ? DateTime.Now : finished),
                    AssignedUser = ticketActivityAssignedUser
                };
        }

        private static TicketActivityAssignedUser AssignedUser(LeanKitCardHistory historyItem)
        {
            if (historyItem.AssignedUserId > 0 && !historyItem.IsUnassigning)
            {
                return new TicketActivityAssignedUser
                    {
                        Id = historyItem.AssignedUserId,
                        Name = historyItem.AssignedUserFullName,
                        Email = new MailAddress(historyItem.AssignedUserEmailAddres)
                    };
            }

            return TicketActivityAssignedUser.UnAssigned;
        }

        private static DateTime ParseLeanKitHistoryDateTime(string rawDateTime)
        {
            return DateTime.Parse(rawDateTime.Replace(" at", String.Empty));
        }
    }
}
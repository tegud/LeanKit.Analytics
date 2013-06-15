using System;
using System.Net.Mail;

namespace LeanKit.Data.SQL
{
    public class TicketActivityFactory : ICreateTicketActivities
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketActivityFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public TicketActivity Build(TicketActivityRecord current, TicketActivityRecord next)
        {
            var started = current.Date;
            var finished = DateTime.MinValue;

            if (next != null)
            {
                finished = next.Date;
            }

            var assignedUser = TicketAssignedUser.UnAssigned;

            if (current.AssignedUserId > 0)
            {
                assignedUser = new TicketAssignedUser
                    {
                        Id = current.AssignedUserId,
                        Name = current.AssignedUserName,
                        Email = new MailAddress(current.AssignedUserEmail)
                    };
            }

            return new TicketActivity
                {
                    Title = current.Activity,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished),
                    AssignedUser = assignedUser
                };
        }
    }
}
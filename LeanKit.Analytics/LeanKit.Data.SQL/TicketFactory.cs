using System;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.SQL
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public Ticket Build(TicketRecord ticket)
        {
            return new Ticket
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Activities = ticket.Activities.SelectWithNext(BuildActivity)
                };
        }

        private TicketActivity BuildActivity(TicketActivityRecord current, TicketActivityRecord next)
        {
            var started = current.Date;
            var finished = DateTime.MinValue;

            if (next != null)
            {
                finished = next.Date;
            }

            return new TicketActivity
                {
                    Title = current.Title,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished)
                };
        }
    }
}
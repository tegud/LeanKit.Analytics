using System;
using System.Linq;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.SQL
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ICalculateWorkDuration _workDurationFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly ICreateTicketActivities _ticketActivityFactory;

        public TicketFactory(ICalculateWorkDuration workDurationFactory, ICalculateTicketMilestone ticketStartDateFactory, ICreateTicketActivities ticketActivityFactory)
        {
            _workDurationFactory = workDurationFactory;
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketActivityFactory = ticketActivityFactory;
        }

        public ICalculateWorkDuration WorkDurationFactory
        {
            get { return _workDurationFactory; }
        }

        public Ticket Build(TicketRecord ticket)
        {
            var activities = ticket.Activities.SelectWithNext((current, next) => _ticketActivityFactory.Build(current, next)).ToArray();

            return new Ticket
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Started = _ticketStartDateFactory.CalculateMilestone(activities),
                    Activities = activities
                };
        }
    }

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

            return new TicketActivity
                {
                    Title = current.Title,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished)
                };
        }
    }

    public interface ICreateTicketActivities
    {
        TicketActivity Build(TicketActivityRecord current, TicketActivityRecord next);
    }
}
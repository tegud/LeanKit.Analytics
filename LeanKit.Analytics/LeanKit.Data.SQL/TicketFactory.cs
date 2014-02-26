using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using LeanKit.Utilities.Collections;

namespace LeanKit.Data.SQL
{
    public class TicketFactory : ICreateTickets
    {
        private readonly ICalculateWorkDuration _ticketCycleTimeDurationFactory;
        private readonly IFindTheCurrentActivity _ticketCurrentActivityFactory;
        private readonly ICalculateTicketMilestone _ticketStartDateFactory;
        private readonly ICalculateTicketMilestone _ticketFinishDateFactory;
        private readonly ICreateTicketActivities _ticketActivityFactory;
        private readonly IMakeTicketBlockages _ticketBlockagesFactory;

        public TicketFactory(ICalculateTicketMilestone ticketStartDateFactory, 
            ICalculateTicketMilestone ticketFinishDateFactory, 
            ICreateTicketActivities ticketActivityFactory, 
            ICalculateWorkDuration ticketCycleTimeDurationFactory, 
            IFindTheCurrentActivity ticketCurrentActivityFactory, 
            IMakeTicketBlockages ticketBlockagesFactory)
        {
            _ticketStartDateFactory = ticketStartDateFactory;
            _ticketFinishDateFactory = ticketFinishDateFactory;
            _ticketActivityFactory = ticketActivityFactory;
            _ticketCycleTimeDurationFactory = ticketCycleTimeDurationFactory;
            _ticketCurrentActivityFactory = ticketCurrentActivityFactory;
            _ticketBlockagesFactory = ticketBlockagesFactory;
        }

        public Ticket Build(TicketRecord ticket)
        {
            var activities = ticket.Activities.SelectWithPreviousAndNext((current, previous, next) => _ticketActivityFactory.Build(current, next)).ToArray();

            var started = _ticketStartDateFactory.CalculateMilestone(activities);
            var finished = _ticketFinishDateFactory.CalculateMilestone(activities);

            if (started == DateTime.MinValue && finished > DateTime.MinValue)
            {
                started = finished;
            }

            var duration = _ticketCycleTimeDurationFactory.CalculateDuration(started, finished);
            var currentActivity = _ticketCurrentActivityFactory.Build(activities);
            
            return new Ticket
                {
                    Id = ticket.Id,
                    ExternalId = ticket.ExternalId,
                    Title = ticket.Title,
                    Started = started,
                    Finished = finished,
                    CycleTime = duration,
                    Size = ticket.Size,
                    Activities = activities,
                    CurrentActivity = currentActivity,
                    AssignedUsers = BuildAssignedUsers(ticket),
                    Release = BuildReleaseInfo(ticket),
                    Blockages = _ticketBlockagesFactory.Build(ticket.Blockages)
                };
        }

        private static IEnumerable<TicketAssignedUser> BuildAssignedUsers(TicketRecord ticket)
        {
            return ticket.AssignedUsers.Select(u => new TicketAssignedUser
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = new MailAddress(u.Email)
                });
        }

        private static TicketReleaseInfo BuildReleaseInfo(TicketRecord ticket)
        {
            return new TicketReleaseInfo
                {
                    Id = ticket.Release == null ? 0 : ticket.Release.Id,
                    SvnRevision = ticket.Release == null ? "" : ticket.Release.SvnRevision,
                    ServiceNowId = ticket.Release == null ? "" : ticket.Release.ServiceNowId
                };
        }
    }
}
﻿using System;
using System.Globalization;
using System.Linq;
using LeanKit.Data;
using LeanKit.Data.SQL;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeViewModelFactory : IBuildCycleTimeViewModels
    {
        private readonly IGetReleasedTicketsFromTheDatabase _ticketRepository;
        private readonly IMakeCycleTimeReleaseViewModels _cycleTimeReleaseViewModelFactory;

        public CycleTimeViewModelFactory(IGetReleasedTicketsFromTheDatabase ticketRepository, IMakeCycleTimeReleaseViewModels cycleTimeReleaseViewModelFactory)
        {
            _cycleTimeReleaseViewModelFactory = cycleTimeReleaseViewModelFactory;
            _ticketRepository = ticketRepository;
        }

        public CycleTimeViewModel Build()
        {
            var tickets = _ticketRepository.Get();

            return new CycleTimeViewModel
            {
                Tickets = tickets.Select(t => new CycleTimeTicketItem
                    {
                        Id = t.Id,
                        ExternalId = t.ExternalId,
                        Title = t.Title,
                        StartedFriendlyText = t.Started.ToFriendlyText("dd MMM yyyy", " HH:mm"),
                        Release = _cycleTimeReleaseViewModelFactory.Build(t),
                        FinishedFriendlyText = t.Finished.ToFriendlyText("dd MMM yyyy", " HH:mm"),
                        Duration = GetDurationText(t),
                        Size = GetSize(t)
                    })
            };
        }

        private static string GetSize(Ticket t)
        {
            if (t.Size == 0)
            {
                return "?";
            }

            return t.Size.ToString();
        }

        private static string GetDurationText(Ticket ticket)
        {
            var isPlural = ticket.CycleTime.Days != 1;
            var suffix = (isPlural ? "s" : "");

            return string.Format("{0} Day{1}", ticket.CycleTime.Days, suffix);
        }
    }

    public interface IMakeCycleTimeReleaseViewModels
    {
        CycleTimeReleaseViewModel Build(Ticket ticket);
    }

    public class CycleTimeReleaseViewModelFactory : IMakeCycleTimeReleaseViewModels
    {
        public CycleTimeReleaseViewModel Build(Ticket ticket)
        {
            if (ticket.Release == null || ticket.Release.Id < 1)
            {
                return CycleTimeReleaseViewModel.NotReleased;
            }

            var name = ticket.Release.Id.ToString();

            if (!String.IsNullOrWhiteSpace(ticket.Release.SvnRevision))
            {
                name = ticket.Release.SvnRevision;
            }
            else if (!String.IsNullOrWhiteSpace(ticket.Release.ServiceNowId))
            {
                name = ticket.Release.ServiceNowId;
            }

            return new CycleTimeReleaseViewModel
                {
                    Id = ticket.Release.Id,
                    Name = name
                };
        }
    }

    public class CycleTimeReleaseViewModel
    {
        public static CycleTimeReleaseViewModel NotReleased = new CycleTimeReleaseViewModel();

        public string Name { get; set; }

        public int Id { get; set; }
    }
}
using System;
using LeanKit.Data;

namespace LeanKit.ReleaseManager.Models
{
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
}
using System;
using LeanKit.Data;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeReleaseViewModelFactory : IMakeCycleTimeReleaseViewModels
    {
        public CycleTimeReleaseViewModel Build(TicketReleaseInfo ticketReleaseInfo)
        {
            if (ticketReleaseInfo == null || ticketReleaseInfo.Id < 1)
            {
                return CycleTimeReleaseViewModel.NotReleased;
            }

            var name = ticketReleaseInfo.Id.ToString();

            if (!String.IsNullOrWhiteSpace(ticketReleaseInfo.SvnRevision))
            {
                name = ticketReleaseInfo.SvnRevision;
            }
            else if (!String.IsNullOrWhiteSpace(ticketReleaseInfo.ServiceNowId))
            {
                name = ticketReleaseInfo.ServiceNowId;
            }

            return new CycleTimeReleaseViewModel
                {
                    Id = ticketReleaseInfo.Id,
                    Name = name
                };
        }
    }
}
using LeanKit.Data;
using LeanKit.ReleaseManager.Models.CycleTime;

namespace LeanKit.ReleaseManager.Models
{
    public interface IMakeCycleTimeReleaseViewModels
    {
        CycleTimeReleaseViewModel Build(TicketReleaseInfo ticketReleaseInfo);
    }
}
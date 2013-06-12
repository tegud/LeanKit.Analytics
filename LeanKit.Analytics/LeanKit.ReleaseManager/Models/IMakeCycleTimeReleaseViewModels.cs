using LeanKit.Data;

namespace LeanKit.ReleaseManager.Models
{
    public interface IMakeCycleTimeReleaseViewModels
    {
        CycleTimeReleaseViewModel Build(Ticket ticket);
    }
}
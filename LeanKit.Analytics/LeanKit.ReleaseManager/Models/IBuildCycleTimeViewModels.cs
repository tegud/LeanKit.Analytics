using LeanKit.Data;
using LeanKit.ReleaseManager.Controllers;
using LeanKit.ReleaseManager.Models.CycleTime;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildCycleTimeViewModels
    {
        CycleTimeViewModel Build(CycleTimeQuery query);
    }
}
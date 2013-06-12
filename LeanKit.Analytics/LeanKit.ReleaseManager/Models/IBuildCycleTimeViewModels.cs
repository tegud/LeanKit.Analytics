using LeanKit.ReleaseManager.Controllers;

namespace LeanKit.ReleaseManager.Models
{
    public interface IBuildCycleTimeViewModels
    {
        CycleTimeViewModel Build(CycleTimeQuery query);
    }
}
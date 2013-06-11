namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeViewModelFactory : IBuildCycleTimeViewModels
    {
        public CycleTimeViewModel Build()
        {
            return new CycleTimeViewModel();
        }
    }
}
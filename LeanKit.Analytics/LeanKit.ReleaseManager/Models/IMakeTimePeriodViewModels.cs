namespace LeanKit.ReleaseManager.Models
{
    public interface IMakeTimePeriodViewModels
    {
        CycleTimePeriodViewModel Build(string selectedPeriod);
    }
}
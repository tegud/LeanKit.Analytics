namespace LeanKit.ReleaseManager.Models
{
    public interface IMakeCycleTimeQueries
    {
        CycleTimeQuery Build(string timePeriod);
    }
}
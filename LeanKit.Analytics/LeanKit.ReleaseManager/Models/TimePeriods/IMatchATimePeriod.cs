namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public interface IMatchATimePeriod
    {

        TimePeriodMatch GetTimeSpanIfMatch(string timePeriod);
    }
}
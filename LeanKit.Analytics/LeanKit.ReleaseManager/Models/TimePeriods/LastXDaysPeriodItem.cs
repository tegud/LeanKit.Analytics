namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class LastXDaysPeriodItem : IDefineATimePeriodItem
    {
        private readonly int _daysBefore;

        public LastXDaysPeriodItem(int daysBefore)
        {
            _daysBefore = daysBefore;
        }

        public string GetLabel()
        {
            return string.Format("Last {0} Days", _daysBefore);
        }

        public string GetValue()
        {
            return string.Format("{0}", _daysBefore);
        }
    }
}
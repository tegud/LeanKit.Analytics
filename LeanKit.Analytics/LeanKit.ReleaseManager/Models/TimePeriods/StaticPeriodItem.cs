namespace LeanKit.ReleaseManager.Models.TimePeriods
{
    public class StaticPeriodItem : IDefineATimePeriodItem
    {
        private readonly string _label;
        private readonly string _value;

        public StaticPeriodItem(string label, string value)
        {
            _label = label;
            _value = value;
        }

        public string GetLabel()
        {
            return _label;
        }

        public string GetValue()
        {
            return _value;
        }
    }
}
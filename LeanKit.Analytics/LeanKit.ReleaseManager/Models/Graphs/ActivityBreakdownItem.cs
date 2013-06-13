namespace LeanKit.ReleaseManager.Models.Graphs
{
    public class ActivityBreakdownItem
    {
        public string Activity { get; set; }

        public string TimeSummary
        {
            get
            {
                return string.Format("{0} hr{1}", Hours, Hours != 1 ? "s" : "");
            }
        }

        public int Days { get; set; }

        public int Hours { get; set; }

        public double Percent { get; set; }

        public string FormattedPercent { get { return Percent.ToString("0.00"); } }

        public bool IsWaste { get; set; }

        public string ClassName
        {
            get { return Activity.Replace(" ", string.Empty); }
        }
    }
}
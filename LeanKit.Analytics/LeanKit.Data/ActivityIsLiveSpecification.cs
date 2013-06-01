namespace LeanKit.Data
{
    public class ActivityIsLiveSpecification : IActivitySpecification
    {
        public bool IsSatisfiedBy(TicketActivity activity)
        {
            var title = activity.Title.ToUpper();
            return title == "LIVE" || title == "WASTE";
        }
    }
}
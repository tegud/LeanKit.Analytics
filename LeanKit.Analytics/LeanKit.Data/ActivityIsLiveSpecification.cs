namespace LeanKit.Data
{
    public class ActivityIsLiveSpecification : IActivitySpecification
    {
        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return activity.Title.ToUpper() == "LIVE";
        }
    }
}
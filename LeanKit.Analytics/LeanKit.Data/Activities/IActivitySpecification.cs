namespace LeanKit.Data.Activities
{
    public interface IActivitySpecification
    {
        bool IsSatisfiedBy(TicketActivity activity);
    }
}
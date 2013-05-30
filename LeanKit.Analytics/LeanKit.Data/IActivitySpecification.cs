namespace LeanKit.Data
{
    public interface IActivitySpecification
    {
        bool IsSatisfiedBy(TicketActivity activity);
    }
}
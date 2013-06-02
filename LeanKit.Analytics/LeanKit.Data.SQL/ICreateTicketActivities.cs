namespace LeanKit.Data.SQL
{
    public interface ICreateTicketActivities
    {
        TicketActivity Build(TicketActivityRecord current, TicketActivityRecord next);
    }
}
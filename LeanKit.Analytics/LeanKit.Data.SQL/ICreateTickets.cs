namespace LeanKit.Data.SQL
{
    public interface ICreateTickets
    {
        Ticket Build(TicketRecord ticket);
    }
}
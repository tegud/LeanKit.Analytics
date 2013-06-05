namespace LeanKit.Data.SQL
{
    public interface ITicketRepository
    {
        void Save(Ticket ticket);
        AllTicketsForBoard GetAll();
    }
}
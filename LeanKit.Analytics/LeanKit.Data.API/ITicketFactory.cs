using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface ITicketFactory
    {
        Ticket Build(LeankitBoardCard card);
    }
}
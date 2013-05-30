using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface ICreateTickets
    {
        Ticket Build(LeankitBoardCard card);
    }
}
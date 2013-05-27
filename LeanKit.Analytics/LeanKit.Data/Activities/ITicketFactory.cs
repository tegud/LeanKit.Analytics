using LeanKit.APIClient.API;

namespace LeanKit.Data.Activities
{
    public interface ITicketFactory
    {
        Ticket Build(LeankitBoardCard card);
    }
}
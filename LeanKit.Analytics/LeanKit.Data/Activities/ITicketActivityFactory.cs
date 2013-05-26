using LeanKit.APIClient.API;

namespace LeanKit.Data.Activities
{
    public interface ITicketActivityFactory
    {
        TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory nextItem);
    }
}
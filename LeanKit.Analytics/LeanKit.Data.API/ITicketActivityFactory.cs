using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface ITicketActivityFactory
    {
        TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory nextItem);
    }
}
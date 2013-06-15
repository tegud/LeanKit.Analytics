using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface ICreateTicketActivities
    {
        TicketActivity Build(LeanKitCardHistory historyItem, LeanKitCardHistory previousItem, LeanKitCardHistory nextItem);
    }
}
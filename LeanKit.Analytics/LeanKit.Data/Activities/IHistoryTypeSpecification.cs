using LeanKit.APIClient.API;

namespace LeanKit.Data.Activities
{
    public interface IHistoryTypeSpecification
    {
        bool IsSpecified(LeanKitCardHistory historyItem);
    }
}
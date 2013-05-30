using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface IHistoryTypeSpecification
    {
        bool IsSpecified(LeanKitCardHistory historyItem);
    }
}
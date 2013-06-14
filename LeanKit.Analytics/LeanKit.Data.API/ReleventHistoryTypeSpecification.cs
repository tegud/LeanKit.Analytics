using System.Collections.Generic;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class ReleventHistoryTypeSpecification : IHistoryTypeSpecification
    {
        public bool IsSpecified(LeanKitCardHistory historyItem)
        {
            var validHistoryTypes = new List<string>
                {
                    "CardCreationEventDTO", "CardMoveEventDTO", "CardBlockedEventDTO", "UserAssignmentEventDTO"
                };

            return validHistoryTypes.Contains(historyItem.Type);
        }
    }
}
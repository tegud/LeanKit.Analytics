using System.Collections.Generic;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface IMakeTicketBlockages
    {
        IEnumerable<TicketBlockage> Build(IEnumerable<LeanKitCardHistory> cardHistory);
    }
}
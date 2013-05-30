using System.Collections.Generic;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface ITicketActivitiesFactory
    {
        IEnumerable<TicketActivity> Build(LeankitBoardCard card);
    }
}
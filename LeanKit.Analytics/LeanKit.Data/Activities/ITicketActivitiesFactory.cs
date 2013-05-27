using System.Collections.Generic;
using LeanKit.APIClient.API;

namespace LeanKit.Data.Activities
{
    public interface ITicketActivitiesFactory
    {
        IEnumerable<TicketActivity> Build(LeankitBoardCard card);
    }
}
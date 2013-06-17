using System.Collections.Generic;
using LeanKit.Data;

namespace LeanKit.ReleaseManager.Models.CycleTime
{
    public interface IBuildListOfCycleTimeItems
    {
        IEnumerable<CycleTimeTicketItem> Build(IEnumerable<Ticket> tickets);
    }
}
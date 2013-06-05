using System.Collections.Generic;

namespace LeanKit.Data
{
    public interface IFindTheCurrentActivity
    {
        TicketActivity Build(IEnumerable<TicketActivity> activities);
    }
}
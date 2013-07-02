using System.Collections.Generic;

namespace LeanKit.Data.SQL
{
    public interface IGetBlockagesFromTheDatabase
    {
        IEnumerable<TicketBlockage> Get(CycleTimeQuery query);
    }
}
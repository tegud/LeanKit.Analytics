using System.Collections.Generic;

namespace LeanKit.Data.SQL
{
    public interface IGetReleasedTicketsFromTheDatabase
    {
        IEnumerable<Ticket> Get(CycleTimeQuery query);
    }
}
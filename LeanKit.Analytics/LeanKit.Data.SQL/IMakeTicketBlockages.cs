using System.Collections.Generic;

namespace LeanKit.Data.SQL
{
    public interface IMakeTicketBlockages
    {
        IEnumerable<TicketBlockage> Build(IEnumerable<TicketActivityRecord> activityRecords);
    }

    public class MakeTicketBlockages : IMakeTicketBlockages
    {
        public IEnumerable<TicketBlockage> Build(IEnumerable<TicketActivityRecord> activityRecords)
        {
            return new List<TicketBlockage>();
        }
    }
}
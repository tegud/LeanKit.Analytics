using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data.SQL
{
    public interface IMakeTicketBlockages
    {
        IEnumerable<TicketBlockage> Build(IEnumerable<TicketBlockedRecord> activityRecords);
    }

    public class MakeTicketBlockages : IMakeTicketBlockages
    {
        public IEnumerable<TicketBlockage> Build(IEnumerable<TicketBlockedRecord> activityRecords)
        {
            return activityRecords.Select(a => new TicketBlockage
                {
                    Reason = a.Reason,
                    Started = a.Started,
                    Finished = a.Finished
                });
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data.SQL
{
    public interface IMakeTicketBlockages
    {
        IEnumerable<TicketBlockage> Build(IEnumerable<TicketBlockedRecord> activityRecords);
    }

    public class TicketBlockageFactory : IMakeTicketBlockages
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketBlockageFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public IEnumerable<TicketBlockage> Build(IEnumerable<TicketBlockedRecord> activityRecords)
        {
            return activityRecords.Select(a => new TicketBlockage
                {
                    Reason = a.Reason,
                    Started = a.Started,
                    Finished = a.Finished,
                    Duration = _workDurationFactory.CalculateDuration(a.Started, a.Finished)
                });
        }
    }
}
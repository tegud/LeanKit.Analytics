using System;

namespace LeanKit.Data
{
    public class TicketCycleTimeDurationFactory : ICalculateWorkDuration
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketCycleTimeDurationFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public WorkDuration CalculateDuration(DateTime started, DateTime finished)
        {
            var duration = _workDurationFactory.CalculateDuration(started,
                                                                  finished > DateTime.MinValue ? finished : DateTime.Now);
            return duration;
        }
    }
}
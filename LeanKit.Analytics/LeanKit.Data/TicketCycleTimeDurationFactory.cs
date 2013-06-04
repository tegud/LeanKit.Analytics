using System;
using LeanKit.Utilities.DateAndTime;

namespace LeanKit.Data
{
    public class TicketCycleTimeDurationFactory : ICalculateWorkDuration
    {
        private readonly ICalculateWorkDuration _workDurationFactory;
        private readonly IKnowTheCurrentDateAndTime _dateTimeWrapper;

        public TicketCycleTimeDurationFactory(ICalculateWorkDuration workDurationFactory, IKnowTheCurrentDateAndTime dateTimeWrapper)
        {
            _workDurationFactory = workDurationFactory;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public WorkDuration CalculateDuration(DateTime started, DateTime finished)
        {
            var duration = _workDurationFactory.CalculateDuration(started,
                                                                  finished > DateTime.MinValue ? finished : _dateTimeWrapper.Now());
            return duration;
        }
    }
}
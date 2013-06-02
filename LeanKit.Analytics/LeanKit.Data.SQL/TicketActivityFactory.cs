using System;

namespace LeanKit.Data.SQL
{
    public class TicketActivityFactory : ICreateTicketActivities
    {
        private readonly ICalculateWorkDuration _workDurationFactory;

        public TicketActivityFactory(ICalculateWorkDuration workDurationFactory)
        {
            _workDurationFactory = workDurationFactory;
        }

        public TicketActivity Build(TicketActivityRecord current, TicketActivityRecord next)
        {
            var started = current.Date;
            var finished = DateTime.MinValue;

            if (next != null)
            {
                finished = next.Date;
            }

            return new TicketActivity
                {
                    Title = current.Activity,
                    Started = started,
                    Finished = finished,
                    Duration = _workDurationFactory.CalculateDuration(started, finished)
                };
        }
    }
}
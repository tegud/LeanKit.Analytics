using System;
using System.Collections.Generic;

namespace LeanKit.Data
{
    public interface ICalculateTicketMilestone
    {
        DateTime CalculateMilestone(IEnumerable<TicketActivity> ticketActivities);
    }
}
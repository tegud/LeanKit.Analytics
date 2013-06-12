using System;
using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeViewModel
    {
        public IEnumerable<CycleTimeTicketItem> Tickets { get; set; }

        public CycleTimePeriodViewModel CycleTimePeriods { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeViewModel
    {
        public IEnumerable<CycleTimeTicketItem> Tickets { get; set; }

        public CycleTimePeriodViewModel CycleTimePeriods { get; set; }

        public TicketCycleTimeSummary Summary { get; set; }
    }

    public class TicketCycleTimeSummary
    {
        public int TicketCount { get; set; }

        public int AverageCycleTime { get; set; }

        public int MaximumCycleTime { get; set; }

        public int NumberOfTicketsWithNoEstimate { get; set; }

        public IEnumerable<CycleTimeBySize> CycleTimeBySize { get; set; }
    }

    public class CycleTimeBySize
    {
        public string Size { get; set; }

        public double CycleTime { get; set; }
    }
}
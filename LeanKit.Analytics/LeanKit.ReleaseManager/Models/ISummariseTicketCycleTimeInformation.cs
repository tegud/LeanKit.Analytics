using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Data;
using LeanKit.ReleaseManager.Models.CycleTime;

namespace LeanKit.ReleaseManager.Models
{
    public interface ISummariseTicketCycleTimeInformation
    {
        TicketCycleTimeSummary Summarise(IEnumerable<Ticket> tickets);
    }

    public class SummariseTicketCycleTimeInformation : ISummariseTicketCycleTimeInformation
    {
        public TicketCycleTimeSummary Summarise(IEnumerable<Ticket> tickets)
        {
            if(!tickets.Any())
            {
                return new TicketCycleTimeSummary { CycleTimeBySize = new CycleTimeBySize[0] };
            }

            var ticketCount = tickets.Count();

            IEnumerable<CycleTimeBySize> cycleTimesBySize =
                tickets.GroupBy(t => t.Size).OrderBy(t => t.Key).Select(t => new CycleTimeBySize { Size = GetSize(t.Key), CycleTime = CalculateAverageCycleTime(t) });

            return new TicketCycleTimeSummary
                {
                    TicketCount = ticketCount,
                    AverageCycleTime = (int) Math.Round((double)tickets.Sum(t => t.CycleTime.Days) / ticketCount),
                    MaximumCycleTime = tickets.Max(t => t.CycleTime.Days),
                    NumberOfTicketsWithNoEstimate = tickets.Count(t => t.Size == 0),
                    CycleTimeBySize = cycleTimesBySize
                };
        }

        private static string GetSize(int ticketSize)
        {
            if(ticketSize == 0)
            {
                return "?";
            }

            return ticketSize.ToString();
        }

        private static double CalculateAverageCycleTime(IEnumerable<Ticket> tickets)
        {
            var ticketCount = tickets.Count();
            var totalCycleTime = tickets.Sum(t => t.CycleTime.Days);

            return Math.Round(totalCycleTime / (double)ticketCount);
        }
    }
}
using System;

namespace LeanKit.Data.SQL
{
    public class TicketBlockedRecord
    {
        public string Reason { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }
    }
}
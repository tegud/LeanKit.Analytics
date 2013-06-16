using System;

namespace LeanKit.Data
{
    public class TicketBlockage
    {
        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public string Reason { get; set; }
    }
}
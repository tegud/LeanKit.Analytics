using System;

namespace LeanKit.Data
{
    public class TicketActivity
    {
        public string Title { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public WorkDuration Duration { get; set; }
    }
}
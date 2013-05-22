using System;
using System.Collections.Generic;

namespace LeanKit.Data
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Started { get; set; }

        public IEnumerable<TicketActivity> Activities { get; set; }
    }

    public class TicketActivity
    {
        public string Title { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }
    }
}
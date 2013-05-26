using System;
using System.Collections.Generic;
using LeanKit.Data.Activities;

namespace LeanKit.Data
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Started { get; set; }

        public IEnumerable<TicketActivity> Activities { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.ReleaseManager.Models
{
    public class ReleaseViewModel
    {
        public int Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public List<ReleaseTicket> Tickets { get; set; }

        public string DateFriendlyText { get; set; }

        public int TicketCount
        {
            get
            {
                return Tickets.Count();
            }
        }

        public string Color { get; set; }

        public int IncludedTickets { get; set; }

        public ReleaseViewModel()
        {
            Tickets = new List<ReleaseTicket>();
        }
    }
}
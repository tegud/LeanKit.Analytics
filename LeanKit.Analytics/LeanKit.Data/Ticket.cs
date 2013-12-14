using System;
using System.Collections.Generic;

namespace LeanKit.Data
{
    public class Ticket
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public DateTime Started { get; set; }

        public IEnumerable<TicketActivity> Activities { get; set; }

        public WorkDuration CycleTime { get; set; }

        public DateTime Finished { get; set; }

        public TicketActivity CurrentActivity { get; set; }

        public int Size { get; set; }

        public TicketReleaseInfo Release { get; set; }

        public IEnumerable<TicketAssignedUser> AssignedUsers { get; set; }

        public IEnumerable<TicketBlockage> Blockages { get; set; }

        public IEnumerable<TicketProject> Projects { get; set; }
    }

    public class TicketProject
    {
        public string ID { get; set; }
    }
}
using System.Collections.Generic;

namespace LeanKit.Data.SQL
{
    public class TicketRecord
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<TicketActivityRecord> Activities { get; set; }

        public string ExternalId { get; set; }

        public int Size { get; set; }

        public TicketReleaseRecord Release { get; set; }

        public List<TicketAssignedUserRecord> AssignedUsers { get; set; }

        public TicketRecord()
        {
            Activities = new List<TicketActivityRecord>();
            AssignedUsers = new List<TicketAssignedUserRecord>();
        }
    }
}
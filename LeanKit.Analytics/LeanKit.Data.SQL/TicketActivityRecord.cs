using System;

namespace LeanKit.Data.SQL
{
    public class TicketActivityRecord
    {
        public string Activity { get; set; }

        public DateTime Date { get; set; }

        public int AssignedUserId { get; set; }

        public string AssignedUserName { get; set; }

        public string AssignedUserEmail { get; set; }
    }
}
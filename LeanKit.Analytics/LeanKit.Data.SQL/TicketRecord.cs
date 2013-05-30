using System.Collections.Generic;

namespace LeanKit.Data.SQL
{
    public class TicketRecord
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<TicketActivityRecord> Activities { get; set; }

        public TicketRecord()
        {
            Activities = new List<TicketActivityRecord>();
        }
    }
}
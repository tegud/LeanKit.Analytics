using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models
{
    public class LaneColumn
    {
        public string Title { get; set; }

        public IEnumerable<ReleaseTicket> Tickets { get; set; }
    }
}
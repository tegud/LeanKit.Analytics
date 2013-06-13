using System.Collections.Generic;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public class LaneColumn
    {
        public string Title { get; set; }

        public IEnumerable<ReleaseTicket> Tickets { get; set; }
    }
}
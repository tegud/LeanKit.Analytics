using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseDetailViewModel
    {
        public int Id { get; set; }

        public ReleasePlannedTime PlannedTime { get; set; }

        public ReleaseActualTime ActualTime { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }

        public ReleaseStatusViewModel Status { get; set; }

        public IEnumerable<ReleaseDetailIncludedTicketViewModel> IncludedTickets { get; set; }
    }
}
using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseDetailIncludedTicketViewModel
    {
        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string Size { get; set; }

        public IEnumerable<ReleaseDetailTicketApproval> Approvals { get; set; }
    }
}
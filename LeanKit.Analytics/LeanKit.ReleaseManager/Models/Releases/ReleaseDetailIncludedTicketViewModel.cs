using System.Collections.Generic;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseDetailIncludedTicketViewModel
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string Size { get; set; }

        public IEnumerable<ReleaseDetailTicketApproval> Approvals { get; set; }
    }
}
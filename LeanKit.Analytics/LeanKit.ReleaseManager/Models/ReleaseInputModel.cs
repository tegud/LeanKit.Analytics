using System;

namespace LeanKit.ReleaseManager.Models
{
    public class ReleaseInputModel
    {
        public int Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public string PlannedTime { get; set; }

        public int PlannedDuration { get; set; }

        public string SelectedTickets { get; set; }

        public string SvnRevision { get; set; }

        public string ServiceNowId { get; set; }
    }
}
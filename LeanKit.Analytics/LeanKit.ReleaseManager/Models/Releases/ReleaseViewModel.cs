using System;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseViewModel
    {
        public int Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public string DateFriendlyText { get; set; }

        public string Color { get; set; }

        public int IncludedTickets { get; set; }
    }
}
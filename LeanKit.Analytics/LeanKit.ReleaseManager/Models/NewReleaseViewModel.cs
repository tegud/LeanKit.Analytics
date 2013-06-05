using System;

namespace LeanKit.ReleaseManager.Models
{
    public class NewReleaseViewModel
    {
        public DateTime PlannedDate { get; set; }

        public string PlannedTime { get; set; }

        public string SelectedTickets { get; set; }
    }
}
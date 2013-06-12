namespace LeanKit.ReleaseManager.Models
{
    public class CycleTimeTicketItem
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Title { get; set; }

        public string StartedFriendlyText { get; set; }

        public string FinishedFriendlyText { get; set; }

        public string Duration { get; set; }

        public CycleTimeReleaseViewModel Release { get; set; }

        public string Size { get; set; }
    }
}
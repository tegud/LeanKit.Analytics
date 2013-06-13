namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseStatusViewModel
    {
        public bool HasStarted { get; set; }

        public bool HasCompleted { get; set; }

        public string Text { get; set; }

        public string CssClass { get; set; }
    }
}
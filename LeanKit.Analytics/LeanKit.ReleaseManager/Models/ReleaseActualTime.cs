using System;

namespace LeanKit.ReleaseManager.Models
{
    public class ReleaseActualTime
    {
        public string StartedFriendlyText { get; set; }

        public string CompletedFriendlyText { get; set; }

        public double Duration { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
using System;

namespace LeanKit.ReleaseManager.Models.Releases
{
    public class ReleaseActualTime
    {
        public string StartedFriendlyText { get; set; }

        public string CompletedFriendlyText { get; set; }

        public string Duration { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public string StartedAtDateFieldValue { get; set; }

        public string StartedAtTimeFieldValue { get; set; }

        public string CompletedAtDateFieldValue { get; set; }

        public string CompletedAtTimeFieldValue { get; set; }
    }
}
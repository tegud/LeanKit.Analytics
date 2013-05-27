using System;

namespace LeanKit.APIClient.API
{
    public class LeanKitCardHistory
    {
        public int CardId { get; set; }

        public string CardTitle { get; set; }

        public int ToLaneId { get; set; }

        public string ToLaneTitle { get; set; }

        public string Type { get; set; }

        public string UserName { get; set; }

        public string UserFullName { get; set; }

        public string GravatarLink { get; set; }

        public string DateTime { get; set; }

        public string TimeDifference { get; set; }

        public Int64 LastDate { get; set; }

        public bool IsBlocked { get; set; }
    }
}
using System.Collections.Generic;

namespace LeanKit.Data
{
    public class AllTicketsForBoard
    {
        public IEnumerable<Ticket> Tickets { get; set; }

        public IEnumerable<Activity> Lanes { get; set; }
    }

    public class Activity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Index { get; set; }
    }
}
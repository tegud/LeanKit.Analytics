using System;

namespace LeanKit.Data
{
    public class CycleTimeQuery
    {
        public static CycleTimeQuery Empty = new CycleTimeQuery();

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanKit.Data
{
    public class CurrentActivityFactory : IFindTheCurrentActivity
    {
        public TicketActivity Build(IEnumerable<TicketActivity> activities)
        {
            return activities.Last(a => a.Finished.Equals(DateTime.MinValue));
        }
    }
}
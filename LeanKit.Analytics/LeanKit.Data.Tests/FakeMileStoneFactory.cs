using System;
using System.Collections.Generic;

namespace LeanKit.Data.Tests
{
    public class FakeMileStoneFactory : ICalculateTicketMilestone
    {
        private readonly DateTime _mileStone;

        public FakeMileStoneFactory(DateTime mileStone = default(DateTime))
        {
            _mileStone = mileStone;
        }

        public DateTime CalculateMilestone(IEnumerable<TicketActivity> ticketActivities)
        {
            return _mileStone;
        }
    }
}
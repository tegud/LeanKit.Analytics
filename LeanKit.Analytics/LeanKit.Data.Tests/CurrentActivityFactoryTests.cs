using System;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class CurrentActivityFactoryTests
    {
        [Test]
        public void ReturnsTheCurrentActivity()
        {
            var ticketActivities = new[]
                {
                    new TicketActivity { Started = new DateTime(2013, 5, 1), Finished = new DateTime(2013, 5, 2) },
                    new TicketActivity { Started = new DateTime(2013, 5, 2), Finished = DateTime.MinValue }
                };

            Assert.That(new CurrentActivityFactory().Build(ticketActivities), Is.EqualTo(ticketActivities[1]));
        }
    }
}
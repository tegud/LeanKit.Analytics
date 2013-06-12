using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class ActivityIsInProgressSpecificationTests
    {
        [Test]
        public void NonInProgressActivityReturnsFalse()
        {
            var ticketActivity = new TicketActivity
                {
                    Title = "READY FOR DEV"
                };

            Assert.That(new ActivityIsInProgressSpecification().IsSatisfiedBy(ticketActivity), Is.False);
        }

        [TestCase("DEV WIP")]
        [TestCase("DEV DONE")]
        [TestCase("READY FOR TEST")]
        [TestCase("TEST WIP")]
        [TestCase("READY FOR RELEASE")]
        [TestCase("LIVE")]
        public void InProgressActivityReturnsTrue(string activity)
        {
            var ticketActivity = new TicketActivity
            {
                Title = activity
            };

            Assert.That(new ActivityIsInProgressSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }

        [Test]
        public void InProgressActivityReturnsTrueIgnoringCase()
        {
            var ticketActivity = new TicketActivity
            {
                Title = "dev wip"
            };

            Assert.That(new ActivityIsInProgressSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }
    }
}

using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class ActivityIsLiveSpecificationTests
    {
        [Test]
        public void NonLiveActivityReturnsFalse()
        {
            var ticketActivity = new TicketActivity
                {
                    Title = "READY FOR DEV"
                };

            Assert.That(new ActivityIsLiveSpecification().IsSatisfiedBy(ticketActivity), Is.False);
        }

        [Test]
        public void LiveActivityReturnsTrue()
        {
            var ticketActivity = new TicketActivity
                {
                    Title = "LIVE"
                };

            Assert.That(new ActivityIsLiveSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }

        [Test]
        public void LiveActivityReturnsTrueIgnoringCase()
        {
            var ticketActivity = new TicketActivity
            {
                Title = "live"
            };

            Assert.That(new ActivityIsLiveSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }

        [Test]
        public void LiveLegacyActivityReturnsTrueIgnoringCase()
        {
            var ticketActivity = new TicketActivity
            {
                Title = "Live: W/C 16/12/12"
            };

            Assert.That(new ActivityIsLiveSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }

        [Test]
        public void WasteActivityReturnsTrue()
        {
            var ticketActivity = new TicketActivity
                {
                    Title = "waste"
                };

            Assert.That(new ActivityIsLiveSpecification().IsSatisfiedBy(ticketActivity), Is.True);
        }
    }
}
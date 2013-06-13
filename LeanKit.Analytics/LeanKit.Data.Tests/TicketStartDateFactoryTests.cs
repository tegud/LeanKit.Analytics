using System;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class TicketStartDateFactoryTests : IActivitySpecification
    {
        private string _titleToMatch;

        [Test]
        public void ReturnsStartedDateTimeOfActivity()
        {
            var expectedStarteDate = new DateTime(2013, 05, 02);
            var activityIsStartedSpecification = this;

            _titleToMatch = null;
            var ticketActivities = new[]
                {
                    new TicketActivity
                        {
                            Started = expectedStarteDate
                        }
                };

            Assert.That(new TicketStartDateFactory(activityIsStartedSpecification).CalculateMilestone(ticketActivities), Is.EqualTo(expectedStarteDate));
        }

        [Test]
        public void ReturnsStartedDateTimeOfFirstActivityWhichMatchesSpecification()
        {
            var expectedStartDate = new DateTime(2013, 05, 02);
            var activityIsStartedSpecification = this;

            _titleToMatch = "DEV WIP";
            var ticketActivities = new[]
                {
                    new TicketActivity
                        {
                            Title = "READY FOR DEV",
                            Started = DateTime.MinValue
                        },
                    new TicketActivity
                        {
                            Title = "DEV WIP",
                            Started = expectedStartDate
                        },
                    new TicketActivity
                        {
                            Title = "LIVE",
                            Started = DateTime.MinValue
                        }
                };

            Assert.That(new TicketStartDateFactory(activityIsStartedSpecification).CalculateMilestone(ticketActivities), Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void ReturnsStartedDateTimeOfFirstActivityWhichMatchesSpecificationRegardlessOfOrder()
        {
            var expectedStartDate = new DateTime(2013, 05, 02);
            var activityIsStartedSpecification = this;

            _titleToMatch = "DEV WIP";
            var ticketActivities = new[]
                {
                    new TicketActivity
                        {
                            Title = "DEV WIP",
                            Started = new DateTime(2013, 05, 03)
                        },
                    new TicketActivity
                        {
                            Title = "DEV WIP",
                            Started = expectedStartDate
                        }
                };

            Assert.That(new TicketStartDateFactory(activityIsStartedSpecification).CalculateMilestone(ticketActivities), Is.EqualTo(expectedStartDate));
        }

        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return _titleToMatch == null || activity.Title == _titleToMatch;
        }
    }
}
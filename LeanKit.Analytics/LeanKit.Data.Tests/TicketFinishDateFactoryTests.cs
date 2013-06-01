using System;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class TicketFinishDateFactoryTests : IActivitySpecification
    {
        private string _titleToMatch;

        [Test]
        public void ReturnsStartedDateTimeOfActivity()
        {
            var expectedFinishDate = new DateTime(2013, 05, 02);
            var activityIsLiveSpecification = this;

            _titleToMatch = null;
            var ticketActivities = new[]
                {
                    new TicketActivity
                        {
                            Started = expectedFinishDate
                        }
                };

            Assert.That(new TicketFinishDateFactory(activityIsLiveSpecification).CalculateMilestone(ticketActivities), Is.EqualTo(expectedFinishDate));
        }

        [Test]
        public void ReturnsStartedDateTimeOfFirstActivityWhichMatchesSpecification()
        {
            var expectedFinishDate = new DateTime(2013, 05, 02);
            var activityIsLiveSpecification = this;

            _titleToMatch = "LIVE";
            var ticketActivities = new[]
                {
                    new TicketActivity
                        {
                            Title = "DEV WIP",
                            Started = DateTime.MinValue
                        },
                    new TicketActivity
                        {
                            Title = "LIVE",
                            Started = expectedFinishDate
                        },
                    new TicketActivity
                        {
                            Title = "LIVE",
                            Started = DateTime.MinValue
                        }
                };

            Assert.That(new TicketFinishDateFactory(activityIsLiveSpecification).CalculateMilestone(ticketActivities), Is.EqualTo(expectedFinishDate));
        }

        public bool IsSatisfiedBy(TicketActivity activity)
        {
            return _titleToMatch == null || activity.Title == _titleToMatch;
        }
    }
}

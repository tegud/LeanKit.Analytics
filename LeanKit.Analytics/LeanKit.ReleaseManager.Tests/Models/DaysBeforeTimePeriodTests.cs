using System;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class DaysBeforeTimePeriodTests : IKnowTheCurrentDateAndTime
    {
        private DateTime _currentDateTime;

        [Test]
        public void NonMatchingTimePeriodSetsIsMatchFalse()
        {
            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchDaysBeforeTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("").IsMatch, Is.False);
        }

        [TestCase("fdsf231")]
        [TestCase("331fdsfs")]
        public void TimePeriodContainingCharactersOtherThanNumbersSetsIsMatchFalse(string timePeriod)
        {
            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchDaysBeforeTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch(timePeriod).IsMatch, Is.False);
        }

        [Test]
        public void MatchingTimePeriodEndToCurrentDate()
        {
            var expectedEndDate = new DateTime(2013, 4, 3);

            _currentDateTime = expectedEndDate;

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchDaysBeforeTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("0").End, Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void MatchingTimePeriodStartToCurrentDateMinusSpecifiedDays()
        {
            var expectedEndDate = new DateTime(2013, 4, 1);

            _currentDateTime = new DateTime(2013, 4, 3);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchDaysBeforeTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("2").Start, Is.EqualTo(expectedEndDate));
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}

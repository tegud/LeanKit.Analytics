using System;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class MatchWeekCommencingTimePeriodTests : IKnowTheCurrentDateAndTime
    {
        private DateTime _currentDateTime;

        [Test]
        public void NonMatchingTimePeriodSetsIsMatchFalse()
        {
            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchWeekCommencingTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("").IsMatch, Is.False);
        }

        [Test]
        public void MatchingTimePeriodStartToStartOfWeek()
        {
            var expectedStartDate = new DateTime(2013, 3, 17);

            _currentDateTime = new DateTime(2013, 4, 3);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchWeekCommencingTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("wc-2").Start, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void MatchingTimePeriodStartToEndOfWeek()
        {
            var expectedEndDate = new DateTime(2013, 3, 23);

            _currentDateTime = new DateTime(2013, 4, 3);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchWeekCommencingTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("wc-2").End, Is.EqualTo(expectedEndDate));
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}
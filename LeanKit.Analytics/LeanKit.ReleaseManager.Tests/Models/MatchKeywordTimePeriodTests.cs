using System;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class MatchKeywordTimePeriodTests : IKnowTheCurrentDateAndTime
    {
        private DateTime _currentDateTime;

        [Test]
        public void NonMatchingTimePeriodSetsIsMatchFalse()
        {
            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("").IsMatch, Is.False);
        }

        [TestCase("this-week")]
        [TestCase("last-week")]
        public void MatchingTimePeriodSetsIsMatchTrue(string timePeriod)
        {
            _currentDateTime = new DateTime(2013, 6, 4);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch(timePeriod).IsMatch, Is.True);
        }

        [TestCase("this-week", -2)]
        [TestCase("last-week", -9)]
        public void MatchingTimePeriodSetsStartDateToStartOfWeek(string timePeriod, int expectedDaysOffset)
        {
            _currentDateTime = new DateTime(2013, 6, 4);

            DateTime expectedStartDate = _currentDateTime.AddDays(expectedDaysOffset);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch(timePeriod).Start, Is.EqualTo(expectedStartDate));
        }

        [TestCase("this-week", 4)]
        [TestCase("last-week", -3)]
        public void MatchingTimePeriodSetsStartDateToEndOfWeek(string timePeriod, int expectedDaysOffset)
        {
            _currentDateTime = new DateTime(2013, 6, 4);

            DateTime expectedEndDate = _currentDateTime.AddDays(expectedDaysOffset);

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch(timePeriod).End, Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void AllTimeSetsStartToDateTimeMin()
        {
            DateTime expectedEndDate = DateTime.MinValue;

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("all-time").Start, Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void AllTimeSetsEndToDateTimeMin()
        {
            DateTime expectedEndDate = DateTime.MinValue;

            var dateTimeWrapper = this;

            var daysBeforeTimePeriod = new MatchKeywordTimePeriod(dateTimeWrapper);
            Assert.That(daysBeforeTimePeriod.GetTimeSpanIfMatch("all-time").End, Is.EqualTo(expectedEndDate));
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}
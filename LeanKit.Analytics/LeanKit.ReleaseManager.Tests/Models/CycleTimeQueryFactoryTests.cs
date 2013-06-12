using System;
using LeanKit.ReleaseManager.Models;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeQueryFactoryTests : IKnowTheCurrentDateAndTime
    {
        private DateTime _currentDateTime;

        [TestCase("all-time")]
        [TestCase("this-week")]
        public void SetsPeriod(string period)
        {
            _currentDateTime = new DateTime(2013, 6, 5);
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;
            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build(period).Period, Is.EqualTo(period));
        }

        [TestCase(-3)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ThisWeekSetsStartDateToThePreviousSunday(int daysToAdd)
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12).AddDays(daysToAdd);

            var expectedStartDate = new DateTime(2013, 6, 2);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("this-week").Start, Is.EqualTo(expectedStartDate));
        }

        [TestCase(-3)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ThisWeekSetsStartDateToTheNextSaturday(int daysToAdd)
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12).AddDays(daysToAdd);

            var expectedStartDate = new DateTime(2013, 6, 8);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("this-week").End, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void LastWeekSetsStartDateToTheSundayBeforePrevious()
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12);

            var expectedStartDate = new DateTime(2013, 5, 26);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("last-week").Start, Is.EqualTo(expectedStartDate));
        }

        [TestCase("", 30)]
        [TestCase("30", 30)]
        [TestCase("90", 90)]
        public void NumberSetsStartDateToThatNumberOfDaysBeforeCurrentDate(string timePeriod, int numberOfDays)
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12);

            var expectedStartDate = _currentDateTime.Date.AddDays(-numberOfDays);

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build(timePeriod).Start, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void NumberSetsEndDateToCurrentDate()
        {
            _currentDateTime = new DateTime(2013, 6, 5, 13, 44, 12);

            var expectedStartDate = _currentDateTime.Date;

            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeQueryFactory(dateTimeWrapper).Build("30").End, Is.EqualTo(expectedStartDate));
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}

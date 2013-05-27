using System;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class WorkDurationFactoryTests
    {
        [Test]
        public void DurationOfOneDaySetsDaysToOne()
        {
            var start = new DateTime(2013, 5, 1);
            var end = new DateTime(2013, 5, 2);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
            {
                Start = 9,
                End = 17
            }).Build(start, end).Days, Is.EqualTo(1));
        }

        [Test]
        public void DurationWhichStartsAndEndsOnSameDayDaysIsOne()
        {
            var start = new DateTime(2013, 5, 1, 9, 0, 0);
            var end = new DateTime(2013, 5, 1, 10, 0, 0);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
            {
                Start = 9,
                End = 17
            }).Build(start, end).Days, Is.EqualTo(1));
        }

        [Test]
        public void DurationOfFridayToMondaySetsDaysToOne()
        {
            var start = new DateTime(2013, 5, 3);
            var end = new DateTime(2013, 5, 6);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9,
                    End = 17
                }).Build(start, end).Days, Is.EqualTo(1));
        }

        [Test]
        public void DurationOfOverAWeekSetsBusinessDays()
        {
            var start = new DateTime(2013, 5, 3);
            var end = new DateTime(2013, 5, 15);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9,
                    End = 17
                }).Build(start, end).Days, Is.EqualTo(8));
        }

        [Test]
        public void DurationOverASpecifiedHolidayIgnoresThatDay()
        {
            var start = new DateTime(2013, 5, 3);
            var end = new DateTime(2013, 5, 15);

            var holidays = new[] { new DateTime(2013, 5, 6) };

            Assert.That(new WorkDurationFactory(holidays, new WorkDayDefinition
                {
                    Start = 9,
                    End = 17
                }).Build(start, end).Days, Is.EqualTo(7));
        }

        [Test]
        public void DurationOfOneHourSetsHours()
        {
            var start = new DateTime(2013, 5, 3, 4, 0, 0);
            var end = new DateTime(2013, 5, 3, 5, 0, 0);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9,
                    End = 17
                }).Build(start, end).Hours, Is.EqualTo(1));
        }

        [Test]
        public void DurationOverOneDayOnlyIncludesWorkingHours()
        {
            var start = new DateTime(2013, 5, 1, 11, 0, 0);
            var end = new DateTime(2013, 5, 3, 16, 0, 0);

            Assert.That(new WorkDurationFactory(new DateTime[0], new WorkDayDefinition
                {
                    Start = 9,
                    End = 17
                }).Build(start, end).Hours, Is.EqualTo(21));
        }
    }
}

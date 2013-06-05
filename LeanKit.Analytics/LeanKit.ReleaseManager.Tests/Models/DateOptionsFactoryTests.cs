using System;
using System.Linq;
using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class DateOptionsFactoryTests : IIdentifyWorkDays
    {
        private DateTime[] _weekendDays;

        [Test]
        public void FirstEntryIsTodaysDate()
        {
            var expectedDate = DateTime.Now.Date;

            _weekendDays = null;

            var dayIsWeekDaySpecification = this;
            var buildDateOptions = new DateOptionsFactory(dayIsWeekDaySpecification).BuildDateOptions(1);

            Assert.That(buildDateOptions.First().Date, Is.EqualTo(expectedDate));
        }

        [Test]
        public void FirstEntryFriendlyTextIsToday()
        {
            const string expectedFriendyName = "Today";

            _weekendDays = null;

            var dayIsWeekDaySpecification = this;
            var buildDateOptions = new DateOptionsFactory(dayIsWeekDaySpecification).BuildDateOptions(1);

            Assert.That(buildDateOptions.First().FriendlyText, Is.EqualTo(expectedFriendyName));
        }

        [Test]
        public void EntryAfterTomorrowFriendyTextIsDayName()
        {
            var today = DateTime.Now.Date;
            var expectedFriendlyDate = today.AddDays(2).DayOfWeek.ToString();

            _weekendDays = null;

            var dayIsWeekDaySpecification = this;
            var buildDateOptions = new DateOptionsFactory(dayIsWeekDaySpecification).BuildDateOptions(3);

            Assert.That(buildDateOptions.Last().FriendlyText, Is.EqualTo(expectedFriendlyDate));
        }

        [Test]
        public void WhenNumberOfDaysForListIsTwoTomorrowIsAdded()
        {
            var expectedDate = DateTime.Now.Date.AddDays(1);

            _weekendDays = null;

            var dayIsWeekDaySpecification = this;
            var buildDateOptions = new DateOptionsFactory(dayIsWeekDaySpecification).BuildDateOptions(2);

            Assert.That(buildDateOptions.Last().Date, Is.EqualTo(expectedDate));
        }

        [Test]
        public void WeekendDaysAreNotIncluded()
        {
            var today = DateTime.Now.Date;
            var expectedDate = today.AddDays(3);

            _weekendDays = new[]
                {
                    today.AddDays(1),
                    today.AddDays(2)
                };

            var dayIsWeekDaySpecification = this;
            var buildDateOptions = new DateOptionsFactory(dayIsWeekDaySpecification).BuildDateOptions(2);

            Assert.That(buildDateOptions.Last().Date, Is.EqualTo(expectedDate));
        }

        public bool IsSatisfiedBy(DateTime date)
        {
            if (_weekendDays != null)
            {
                return !_weekendDays.Contains(date);
            }

            return true;
        }
    }
}

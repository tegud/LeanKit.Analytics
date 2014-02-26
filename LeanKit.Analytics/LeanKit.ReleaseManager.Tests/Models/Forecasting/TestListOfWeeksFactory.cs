using System;
using System.Linq;
using LeanKit.ReleaseManager.Models.Forecasting;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models.Forecasting
{
    [TestFixture]
    public class TestListOfWeeksFactory
    {
        [Test]
        public void list_of_one_week_starts_on_provided_date()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 1).First().Start, Is.EqualTo(startOfWeekOne));
        }

        [Test]
        public void list_of_one_week_ends_on_provided_date_plus_six_days()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 1).First().End, Is.EqualTo(startOfWeekOne.AddDays(6)));
        }

        [Test]
        public void list_of_two_weeks_contains_two_weeks()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 2).Count(), Is.EqualTo(2));
        }

        [Test]
        public void list_of_two_weeks_second_week_starts_on_provided_date_plus_seven_days()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 2).ElementAt(1).Start, Is.EqualTo(startOfWeekOne.AddDays(7)));
        }

        [Test]
        public void list_of_two_weeks_second_week_end_on_provided_date_plus_thirteen_days()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 2).ElementAt(1).End, Is.EqualTo(startOfWeekOne.AddDays(13)));
        }

        [Test]
        public void StartOfPeriod_returns_start_of_first_week()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 2).StartOfPeriod, Is.EqualTo(startOfWeekOne));
        }

        [Test]
        public void EndOfPeriod_returns_end_of_last_week()
        {
            var startOfWeekOne = new DateTime(2013, 12, 8);

            Assert.That(new ListOfWeeks(startOfWeekOne, 2).EndOfPeriod, Is.EqualTo(startOfWeekOne.AddDays(13)));
        }
    }
}

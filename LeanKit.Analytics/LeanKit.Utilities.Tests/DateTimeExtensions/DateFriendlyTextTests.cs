using System;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.Utilities.Tests.DateTimeExtensions
{
    [TestFixture]
    public class DateFriendlyTextTests
    {
        [Test]
        public void ReturnsEmptyStringForDateTimeMin()
        {
            Assert.That(DateTime.MinValue.ToFriendlyText(string.Empty, string.Empty), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ReturnsTodayForDateMatchingCurrentDate()
        {
            Assert.That(DateTime.Now.ToFriendlyText(string.Empty, string.Empty), Is.EqualTo("Today"));
        }

        [Test]
        public void ReturnsTomorrowForDateMatchingCurrentDatePlusOneDay()
        {
            Assert.That(DateTime.Now.AddDays(1).ToFriendlyText(string.Empty, string.Empty), Is.StringStarting("Tomorrow"));
        }

        [Test]
        public void ReturnsYesterdayForDateMatchingCurrentDateMinusOneDay()
        {
            Assert.That(DateTime.Now.AddDays(1).ToFriendlyText(string.Empty, string.Empty), Is.StringStarting("Tomorrow"));
        }

        [Test]
        public void ReturnsDateFormattedUsingProvidedStringWhenDateIsADateInThePast()
        {
            Assert.That(new DateTime(1990, 1, 1).ToFriendlyText("dd MMM yyyy", string.Empty), Is.StringStarting("01 Jan 1990"));
        }

        [Test]
        public void ReturnsTimeFormattedToEndOfString()
        {
            Assert.That(new DateTime(1990, 1, 1, 15, 10, 0).ToFriendlyText(string.Empty, "HH:mm"), Is.StringEnding("15:10"));
        }
    }
}
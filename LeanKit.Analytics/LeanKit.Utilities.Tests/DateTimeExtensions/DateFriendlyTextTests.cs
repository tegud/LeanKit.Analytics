using System;
using System.Collections.Generic;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.Utilities.Tests.DateTimeExtensions
{
    [TestFixture]
    public class DateFriendlyTextTests
    {
        [Test]
        public void ReturnsTodayForDateMatchingCurrentDate()
        {
            Assert.That(DateTime.Now.ToFriendlyText("", ""), Is.StringStarting("Today"));
        }

        [Test]
        public void ReturnsTomorrowForDateMatchingCurrentDatePlusOneDay()
        {
            Assert.That(DateTime.Now.AddDays(1).ToFriendlyText("", ""), Is.StringStarting("Tomorrow"));
        }

        [Test]
        public void ReturnsYesterdayForDateMatchingCurrentDateMinusOneDay()
        {
            Assert.That(DateTime.Now.AddDays(1).ToFriendlyText("", ""), Is.StringStarting("Tomorrow"));
        }

        [Test]
        public void ReturnsDateFormattedUsingProvidedStringWhenDateIsADateInThePast()
        {
            Assert.That(new DateTime(1990, 1, 1).ToFriendlyText("dd MMM yyyy", ""), Is.StringStarting("01 Jan 1990"));
        }

        [Test]
        public void ReturnsTimeFormattedToEndOfString()
        {
            Assert.That(new DateTime(1990, 1, 1, 15, 10, 0).ToFriendlyText("", "HH:mm"), Is.StringEnding("15:10"));
        }
    }

    public static class DateFriendTextExtensions
    {
        private static readonly Dictionary<double, string> FriendlyTexts = new Dictionary<double, string>
            {
                { -1, "Yesterday" },
                { 0, "Today" },
                { 1, "Tomorrow" }
            };

        public static string ToFriendlyText (this DateTime date, string dateFormat, string timeFormat)
        {
            var todaysDate = DateTime.Now.Date;
            var daysDiff = (date.Date - todaysDate).TotalDays;

            string friendlyText;

            if (!FriendlyTexts.TryGetValue(daysDiff, out friendlyText))
            {
                friendlyText = date.ToString(dateFormat);
            }

            return friendlyText + date.ToString(timeFormat);
        }
    }
}
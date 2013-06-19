using System;
using System.Linq;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class ProductOwnerDashboardTimePeriodConfigurationTests : IKnowTheCurrentDateAndTime
    {
        [TestCase(0, typeof(MatchWeekCommencingTimePeriod))]
        [TestCase(1, typeof(MatchKeywordTimePeriod))]
        public void SetsMatchers(int index, Type expectedMatcher)
        {
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new ProductOwnerDashboardTimePeriodConfiguration(dateTimeWrapper).Matchers.ElementAt(index), Is.TypeOf(expectedMatcher));
        }

        [Test]
        public void SetsDefaultValue()
        {
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new ProductOwnerDashboardTimePeriodConfiguration(dateTimeWrapper).DefaultValue, Is.EqualTo("this-week"));
        }

        public DateTime Now()
        {
            throw new NotImplementedException();
        }
    }
}
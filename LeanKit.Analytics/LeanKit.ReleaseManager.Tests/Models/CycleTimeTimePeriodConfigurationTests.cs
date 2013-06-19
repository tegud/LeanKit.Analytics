using System;
using System.Linq;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeTimePeriodConfigurationTests : IKnowTheCurrentDateAndTime
    {
        [TestCase(0, typeof(MatchDaysBeforeTimePeriod))]
        [TestCase(1, typeof(MatchKeywordTimePeriod))]
        public void SetsMatchers(int index, Type expectedMatcher)
        {
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeTimePeriodConfiguration(dateTimeWrapper).Matchers.ElementAt(index), Is.TypeOf(expectedMatcher));
        }

        [Test]
        public void SetsDefaultValue()
        {
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            Assert.That(new CycleTimeTimePeriodConfiguration(dateTimeWrapper).DefaultValue, Is.EqualTo("30"));
        }

        public DateTime Now()
        {
            throw new NotImplementedException();
        }
    }
}
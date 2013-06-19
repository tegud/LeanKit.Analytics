using System;
using System.Collections.Generic;
using LeanKit.ReleaseManager.ErrorHandling;
using LeanKit.ReleaseManager.Models.CycleTime;
using LeanKit.ReleaseManager.Models.TimePeriods;
using LeanKit.Utilities.DateAndTime;
using NUnit.Framework;

namespace LeanKit.ReleaseManager.Tests.Models
{
    [TestFixture]
    public class CycleTimeQueryFactoryTests : IConfigureTimePeriods
    {
        private string _defaultValue;
        private IEnumerable<IMatchATimePeriod> _matchers = new IMatchATimePeriod[0];
        private DateTime _startDateForMatch;
        private DateTime _endDateForMatch;

        [Test]
        public void InvalidTimePeriodThrowsUnknownTimePeriodException()
        {
            _matchers = new IMatchATimePeriod[0];
            IConfigureTimePeriods configuration = this;
            Assert.Throws<UnknownTimePeriodException>(() => new CycleTimeQueryFactory(configuration).Build("gfdgfd"));
        }

        [Test]
        public void EmptyTimePeriodUsesDefault()
        {
            _defaultValue = "the-default";

            _matchers = new IMatchATimePeriod[] { new FakeTimePeriodMatcher("the-default", DateTime.MinValue, DateTime.MinValue) };

            IConfigureTimePeriods configuration = this;
            Assert.That(new CycleTimeQueryFactory(configuration).Build("").Period, Is.EqualTo("the-default"));
        }

        [Test]
        public void SetsStartToTimePeriodStart()
        {
            var expectedStartDate = new DateTime(2013, 01, 01);

            _defaultValue = "the-default";

            _matchers = new IMatchATimePeriod[] { new FakeTimePeriodMatcher("the-default", expectedStartDate, DateTime.MinValue) };

            IConfigureTimePeriods configuration = this;
            Assert.That(new CycleTimeQueryFactory(configuration).Build("").Start, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void SetsEndToTimePeriodStart()
        {
            var expectedEndDate = new DateTime(2013, 01, 01);

            _defaultValue = "the-default";

            _matchers = new IMatchATimePeriod[] { new FakeTimePeriodMatcher("the-default", DateTime.MinValue, expectedEndDate) };

            IConfigureTimePeriods configuration = this;
            Assert.That(new CycleTimeQueryFactory(configuration).Build("").End, Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void UsesFirstMatchingTimePeriod()
        {
            var expectedStartDate = new DateTime(2013, 1, 1);

            _matchers = new IMatchATimePeriod[]
                {
                    new FakeTimePeriodMatcher("one", expectedStartDate, DateTime.MinValue),
                    new FakeTimePeriodMatcher("one", DateTime.MinValue, DateTime.MinValue)
                };

            IConfigureTimePeriods configuration = this;
            Assert.That(new CycleTimeQueryFactory(configuration).Build("one").Start, Is.EqualTo(expectedStartDate));
        }

        public IEnumerable<IMatchATimePeriod> Matchers { get { return _matchers; } }
        public string DefaultValue { get { return _defaultValue; }}
    }

    public class FakeTimePeriodMatcher : IMatchATimePeriod
    {
        private readonly string _matchingTimePeriod;
        private readonly DateTime _startDateForMatch;
        private readonly DateTime _endDateForMatch;

        public FakeTimePeriodMatcher(string matchingTimePeriod, DateTime startDateForMatch, DateTime endDateForMatch)
        {
            _matchingTimePeriod = matchingTimePeriod;
            _startDateForMatch = startDateForMatch;
            _endDateForMatch = endDateForMatch;
        }

        public TimePeriodMatch GetTimeSpanIfMatch(string timePeriod)
        {
            if (timePeriod == _matchingTimePeriod)
            {
                return new TimePeriodMatch(_startDateForMatch, _endDateForMatch);
            }

            return TimePeriodMatch.NotMatched;
        }
    }
}

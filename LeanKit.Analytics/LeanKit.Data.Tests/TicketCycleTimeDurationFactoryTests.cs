using System;
using LeanKit.Utilities.DateTime;
using NUnit.Framework;

namespace LeanKit.Data.Tests
{
    [TestFixture]
    public class TicketCycleTimeDurationFactoryTests : ICalculateWorkDuration, IKnowTheCurrentDateAndTime
    {
        private DateTime _actualEnd;
        private DateTime _actualStart;
        private DateTime _currentDateTime;

        [Test]
        public void StartDateIsUsedToCalculateDuration()
        {
            ICalculateWorkDuration workDurationFactory = this;
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            var started = new DateTime();
            var expectedStart = started;

            new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper).CalculateDuration(started, new DateTime());

            Assert.That(_actualStart, Is.EqualTo(expectedStart));
        }

        [Test]
        public void FinishedDateIsUsedToCalculateDuration()
        {
            ICalculateWorkDuration workDurationFactory = this;
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            var finished = new DateTime(2013, 10, 10);
            var expectedFinished = finished;

            new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper).CalculateDuration(new DateTime(), finished);

            Assert.That(_actualEnd, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void CurrentDateIsFinishDateProvidedWhenFinishedIsMinDateTime()
        {
            ICalculateWorkDuration workDurationFactory = this;
            IKnowTheCurrentDateAndTime dateTimeWrapper = this;

            var finished = new DateTime();
            _currentDateTime = new DateTime(2013, 05, 02);
            var expectedFinished = _currentDateTime;

            new TicketCycleTimeDurationFactory(workDurationFactory, dateTimeWrapper).CalculateDuration(new DateTime(), finished);

            Assert.That(_actualEnd, Is.EqualTo(expectedFinished));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            _actualStart = start;
            _actualEnd = end;
            return null;
        }

        public DateTime Now()
        {
            return _currentDateTime;
        }
    }
}
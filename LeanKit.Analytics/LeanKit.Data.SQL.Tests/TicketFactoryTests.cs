using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class TicketFactoryTests : ICalculateWorkDuration, ICreateTicketActivities, ICalculateTicketMilestone
    {
        private WorkDuration _cycleTime;
        private DateTime _mileStoneDate;

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Test Title";

            var ticketActivityFactory = this;
            var cycleTimeFactory = this;
            var ticketStartDateFactory = this;

            Assert.That(new TicketFactory(cycleTimeFactory, ticketStartDateFactory, ticketActivityFactory).Build(new TicketRecord
                {
                    Title = expectedTitle
                }).Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void SetsTicketId()
        {
            const int expectedId = 12345;

            var ticketActivityFactory = this;
            var cycleTimeFactory = this;
            var ticketStartDateFactory = this;

            Assert.That(new TicketFactory(cycleTimeFactory, ticketStartDateFactory, ticketActivityFactory).Build(new TicketRecord
                {
                    Id = expectedId
                }).Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketStarted()
        {
            var expectedStarted = new DateTime(2013, 5, 1);

            _mileStoneDate = expectedStarted;

            var ticketActivityFactory = this;
            var cycleTimeFactory = this;
            var ticketStartDateFactory = this;

            var ticketFactory = new TicketFactory(ticketActivityFactory, ticketStartDateFactory, cycleTimeFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).Started, Is.EqualTo(expectedStarted));
        }

        //[Test]
        //public void SetsTicketCycleTime()
        //{
        //    var expectedCycleTime = new WorkDuration();

        //    _cycleTime = expectedCycleTime;

        //    var ticketActivityFactory = this;
        //    var cycleTimeFactory = this;

        //    Assert.That(new TicketFactory(ticketActivityFactory, cycleTimeFactory).Build(new TicketRecord()).CycleTime, Is.EqualTo(expectedCycleTime));
        //}

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _cycleTime;
        }

        public TicketActivity Build(TicketActivityRecord current, TicketActivityRecord next)
        {
            return null;
        }

        public DateTime CalculateMilestone(IEnumerable<TicketActivity> ticketActivities)
        {
            return _mileStoneDate;
        }
    }

    public class TicketActivityFactoryTests : ICalculateWorkDuration
    {
        private WorkDuration _workDuration;

        [Test]
        public void SetsTicketActivityTitle()
        {
            const string expectedActivityTitle = "Dev WIP";

            var current = new TicketActivityRecord { Title = expectedActivityTitle };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Title, Is.EqualTo(expectedActivityTitle));
        }

        [Test]
        public void SetsTicketActivityStartedToCurrentActivityDate()
        {
            var expectedActivityStarted = new DateTime();

            var current = new TicketActivityRecord { Date = expectedActivityStarted };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Started, Is.EqualTo(expectedActivityStarted));
        }

        [Test]
        public void SetsTicketActivityFinishedToNextActivityDate()
        {
            var expectedActivityFinished = new DateTime(2013, 5, 3);

            var current = new TicketActivityRecord { Date = new DateTime() };
            var next = new TicketActivityRecord { Date = expectedActivityFinished };

            Assert.That(new TicketActivityFactory(this).Build(current, next).Finished, Is.EqualTo(expectedActivityFinished));
        }

        [Test]
        public void SetsTicketActivityFinishedWhenItIsTheCurrentActivity()
        {
            var expectedActivityFinished = new DateTime();

            var current = new TicketActivityRecord { Date = new DateTime(2013, 1, 1) };

            Assert.That(new TicketActivityFactory(this).Build(current, null).Finished, Is.EqualTo(expectedActivityFinished));
        }

        [Test]
        public void SetsTicketActivityDuration()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ICalculateWorkDuration workDurationFactory = this;

            var current = new TicketActivityRecord
                {
                    Date = new DateTime(2013, 05, 01)
                };
            var next = new TicketActivityRecord
                {
                    Date = new DateTime(2013, 05, 01)
                };

            Assert.That(new TicketActivityFactory(workDurationFactory).Build(current, next).Duration, Is.EqualTo(expectedDuration));
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }

        public TicketActivity BuildActivity(TicketActivityRecord current, TicketActivityRecord next)
        {
            throw new NotImplementedException();
        }
    }
}

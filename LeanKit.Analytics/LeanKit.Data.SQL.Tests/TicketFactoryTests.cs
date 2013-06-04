using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class CurrentActivityFactoryTests
    {
        [Test]
        public void ReturnsTheCurrentActivity()
        {
            var ticketActivities = new[]
                {
                    new TicketActivity { Started = new DateTime(2013, 5, 1), Finished = new DateTime(2013, 5, 2) },
                    new TicketActivity { Started = new DateTime(2013, 5, 2), Finished = DateTime.MinValue }
                };

            Assert.That(new CurrentActivityFactory().Build(ticketActivities), Is.EqualTo(ticketActivities[1]));
        }
    }

    [TestFixture]
    public class TicketFactoryTests : ICalculateWorkDuration, ICreateTicketActivities, ICalculateTicketMilestone, IFindTheCurrentActivity
    {
        private WorkDuration _cycleTime;
        private DateTime _mileStoneDate;
        private TicketActivity _currentActivity;

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Test Title";

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
                {
                    Title = expectedTitle
                }).Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void SetsTicketId()
        {
            const int expectedId = 12345;

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            Assert.That(new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory).Build(new TicketRecord
                {
                    Id = expectedId
                }).Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketStarted()
        {
            var expectedStarted = new DateTime(2013, 5, 1);

            _mileStoneDate = expectedStarted;

            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).Started, Is.EqualTo(expectedStarted));
        }

        [Test]
        public void SetsTicketFinished()
        {
            var expectedFinished = new DateTime(2013, 5, 1);

            _mileStoneDate = expectedFinished;

            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsTicketCycleTime()
        {
            var expectedCycleTime = new WorkDuration();

            _cycleTime = expectedCycleTime;

            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).CycleTime, Is.EqualTo(expectedCycleTime));
        }

        [Test]
        public void SetsTicketCurrentActivity()
        {
            var expectedCurrentActivity = new TicketActivity();

            _currentActivity = expectedCurrentActivity;

            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).CurrentActivity, Is.EqualTo(expectedCurrentActivity));
        }

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

        public TicketActivity Build(IEnumerable<TicketActivity> activities)
        {
            return _currentActivity;
        }
    }
}

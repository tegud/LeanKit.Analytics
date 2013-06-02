using System;
using System.Collections.Generic;
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
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var ticketFactory = new TicketFactory(cycleTimeFactory, ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory);

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
            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;

            Assert.That(new TicketFactory(cycleTimeFactory, ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory).Build(new TicketRecord
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
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var ticketFactory = new TicketFactory(ticketActivityFactory, ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).Started, Is.EqualTo(expectedStarted));
        }

        [Test]
        public void SetsTicketFinished()
        {
            var expectedFinished = new DateTime(2013, 5, 1);

            _mileStoneDate = expectedFinished;

            var ticketActivityFactory = this;
            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var ticketFactory = new TicketFactory(ticketActivityFactory, ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsTicketCycleTime()
        {
            var expectedCycleTime = new WorkDuration();

            _cycleTime = expectedCycleTime;

            var ticketActivityFactory = this;
            var cycleTimeFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;

            var ticketFactory = new TicketFactory(ticketActivityFactory, ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).CycleTime, Is.EqualTo(expectedCycleTime));
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
    }
}

using System;
using System.Collections.Generic;
using LeanKit.APIClient.API;
using LeanKit.Data.Tests;
using NUnit.Framework;

namespace LeanKit.Data.API.Tests
{
    [TestFixture]
    public class TicketFactoryTests : ITicketActivitiesFactory, ICalculateWorkDuration
    {
        private WorkDuration _workDuration;

        [Test]
        public void SetsId()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Id = 12345 }).Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsTitle()
        {
            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard { Title = "Test Title" }).Title, Is.EqualTo("Test Title"));
        }

        [Test]
        public void SetsStarted()
        {
            var expectedStartDate = new DateTime(2013, 02, 15, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, new FakeMileStoneFactory(expectedStartDate), fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard()).Started, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void SetsFinished()
        {
            var expectedFinished = new DateTime(2013, 02, 16, 10, 50, 35);

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, new FakeMileStoneFactory(expectedFinished));

            Assert.That(ticketFactory.Build(new LeankitBoardCard()).Finished, Is.EqualTo(expectedFinished));
        }

        [Test]
        public void SetsCycleTime()
        {
            var expectedDuration = new WorkDuration();

            _workDuration = expectedDuration;

            ITicketActivitiesFactory ticketActivitiesFactory = this;
            ICalculateWorkDuration ticketCycleTimeDurationFactory = this;

            var fakeMileStoneFactory = new FakeMileStoneFactory();

            var ticketFactory = new TicketFactory(ticketActivitiesFactory, ticketCycleTimeDurationFactory, fakeMileStoneFactory, fakeMileStoneFactory);

            Assert.That(ticketFactory.Build(new LeankitBoardCard()).CycleTime, Is.EqualTo(expectedDuration));
        }

        public IEnumerable<TicketActivity> Build(LeankitBoardCard card)
        {
            return new List<TicketActivity>(0);
        }

        public WorkDuration CalculateDuration(DateTime start, DateTime end)
        {
            return _workDuration;
        }
    }
}
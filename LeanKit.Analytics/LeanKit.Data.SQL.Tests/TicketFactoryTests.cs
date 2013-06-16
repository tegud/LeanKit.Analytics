using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class TicketFactoryTests : ICalculateWorkDuration, ICreateTicketActivities, ICalculateTicketMilestone, IFindTheCurrentActivity, IMakeTicketBlockages
    {
        private WorkDuration _cycleTime;
        private DateTime _mileStoneDate;
        private TicketActivity _currentActivity;
        private IEnumerable<TicketBlockage> _blockages;

        [Test]
        public void SetsTicketTitle()
        {
            const string expectedTitle = "Test Title";

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

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
            var ticketBlockagesFactory = this;

            Assert.That(new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory).Build(new TicketRecord
            {
                Id = expectedId
            }).Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void SetsTicketSize()
        {
            const int expectedSize = 2;

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            Assert.That(new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory).Build(new TicketRecord
            {
                Size = expectedSize
            }).Size, Is.EqualTo(expectedSize));
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
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

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
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

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
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

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
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, cycleTimeFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord()).CurrentActivity, Is.EqualTo(expectedCurrentActivity));
        }

        [Test]
        public void SetsTicketExternalId()
        {
            const string expectedExternalId = "R-12345";

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                ExternalId = expectedExternalId
            }).ExternalId, Is.EqualTo(expectedExternalId));
        }

        [Test]
        public void SetsTicketReleaseId()
        {
            const int expectedReleaseId = 12345;

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord { Id = expectedReleaseId }
            }).Release.Id, Is.EqualTo(expectedReleaseId));
        }

        [Test]
        public void SetsTicketReleaseSvnRevision()
        {
            const string expectedSvnRevision = "84324";

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord { SvnRevision = expectedSvnRevision }
            }).Release.SvnRevision, Is.EqualTo(expectedSvnRevision));
        }

        [Test]
        public void SetsTicketReleaseServiceNowId()
        {
            const string expectedServiceNowId = "CHG000123";

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord { ServiceNowId = expectedServiceNowId }
            }).Release.ServiceNowId, Is.EqualTo(expectedServiceNowId));
        }

        [Test]
        public void SetsAssignedUsersId()
        {
            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord(),
                AssignedUsers = new List<TicketAssignedUserRecord>
                    {
                        new TicketAssignedUserRecord { Id = 12345, Email = "developer@example.com" }
                    }
            }).AssignedUsers.First().Id, Is.EqualTo(12345));
        }

        [Test]
        public void SetsAssignedUsersName()
        {
            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord(),
                AssignedUsers = new List<TicketAssignedUserRecord>
                    {
                        new TicketAssignedUserRecord { Name = "Mr Developer", Email = "developer@example.com" }
                    }
            }).AssignedUsers.First().Name, Is.EqualTo("Mr Developer"));
        }

        [Test]
        public void SetsAssignedUsersEmail()
        {
            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(new TicketRecord
            {
                Release = new TicketReleaseRecord(),
                AssignedUsers = new List<TicketAssignedUserRecord>
                    {
                        new TicketAssignedUserRecord { Email = "developer@example.com" }
                    }
            }).AssignedUsers.First().Email.Address, Is.EqualTo("developer@example.com"));
        }

        [Test]
        public void SetsBlockages()
        {
            _blockages = new List<TicketBlockage>();
            var ticketRecord = new TicketRecord();

            var ticketActivityFactory = this;
            var ticketMilestoneFactory = this;
            var ticketCycleTimeDurationFactory = this;
            var ticketCurrentActivityFactory = this;
            var ticketBlockagesFactory = this;

            var ticketFactory = new TicketFactory(ticketMilestoneFactory, ticketMilestoneFactory, ticketActivityFactory, ticketCycleTimeDurationFactory, ticketCurrentActivityFactory, ticketBlockagesFactory);

            Assert.That(ticketFactory.Build(ticketRecord).Blockages, Is.EqualTo(_blockages));
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

        public IEnumerable<TicketBlockage> Build(IEnumerable<TicketActivityRecord> activityRecords)
        {
            return _blockages;
        }
    }
}
